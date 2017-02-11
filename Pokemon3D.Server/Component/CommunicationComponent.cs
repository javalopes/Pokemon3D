using System;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Pokemon3D.Server.Component
{
    class CommunicationComponent : IServerComponent
    {
        private NetServer _server;

        public string Name => "Network Communication";

        public CommunicationComponent()
        {
            var configuration = new NetPeerConfiguration("Pokemon3D.Network.Communcation.Server");
            _server = new NetServer(configuration);
        }

        public bool Start()
        {
            _server.Start();

            Task.Factory.StartNew(HandleNetMessages);

            return true;
        }

        private void HandleNetMessages()
        {
            while (true)
            {
                if (_server.Status == NetPeerStatus.ShutdownRequested) break;
                if (_server.Status == NetPeerStatus.NotRunning) break;

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

        public void Stop()
        {
            _server.Shutdown("Shutdown from server");
        }
    }
}
