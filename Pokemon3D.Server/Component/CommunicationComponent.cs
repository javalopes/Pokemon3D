using System;
using System.Threading;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Pokemon3D.Server.Component
{
    class CommunicationComponent : AsyncComponentBase
    {
        private readonly NetServer _server;

        public override string Name => "Network Communication";

        public CommunicationComponent(IMessageBroker messageBroker) : base(messageBroker)
        {
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
                        case NetIncomingMessageType.VerboseDebugMessage:
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.ErrorMessage:
                            Console.WriteLine(msg.ReadString());
                            break;
                        default:
                            Console.WriteLine("Unhandled type: " + msg.MessageType);
                            break;
                    }
                    _server.Recycle(msg);
                }
            }
        }
    }
}
