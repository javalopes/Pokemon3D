using System.Threading;
using Lidgren.Network;
using Pokemon3D.Server.Management;

namespace Pokemon3D.Server.Component
{
    class CommunicationComponent : AsyncComponentBase
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IClientRegistrator _registrator;
        private readonly NetServer _server;

        public override string Name => "Network Communication";

        public CommunicationComponent(IMessageBroker messageBroker, IClientRegistrator registrator) : base(messageBroker)
        {
            _messageBroker = messageBroker;
            _registrator = registrator;
            var configuration = new NetPeerConfiguration("Pokemon3D.Network.Communcation.Server");
            _server = new NetServer(configuration);
        }

        protected override void OnStart()
        {
            _server.Start();
        }

        protected override void OnExecute(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    _server.Shutdown("Shutdown from server");
                    break;
                }

                NetIncomingMessage msg;
                while ((msg = _server.ReadMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            OnConnectionApproval(msg);
                            break;
                        case NetIncomingMessageType.VerboseDebugMessage:
                            _messageBroker.Notify("Network VerboseDebugMsg: " + msg.ReadString());
                            break;
                        case NetIncomingMessageType.DebugMessage:
                            _messageBroker.Notify("Network DebugMsg: " + msg.ReadString());
                            break;
                        case NetIncomingMessageType.WarningMessage:
                            _messageBroker.Notify("Network WarningMsg: " + msg.ReadString());
                            break;
                        case NetIncomingMessageType.ErrorMessage:
                            _messageBroker.Notify("Network ErrorMsg: " + msg.ReadString());
                            break;
                        default:
                            _messageBroker.Notify("Unhandled type: " + msg.MessageType);
                            break;
                    }
                    _server.Recycle(msg);
                }
            }
        }

        private void OnConnectionApproval(NetIncomingMessage msg)
        {
            _registrator.RegisterClient(new Player(msg.ReadString()));
        }
    }
}
