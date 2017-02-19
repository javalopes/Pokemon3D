using System;
using System.Threading;
using Lidgren.Network;

namespace Pokemon3D.Networking.Client
{
    public class GameNetworkClient
    {
        private readonly NetClient _netClient;

        public string HostName { get; }

        public int Port { get; }

        public NetworkClientState State { get; private set; }

        public string ErrorMessage { get; private set; }

        public Guid ClientUniqueId { get; private set; }

        public GameNetworkClient(string hostName, int port)
        {
            HostName = hostName;
            Port = port;
            State = NetworkClientState.Disconnected;

            var configuration = new NetPeerConfiguration(NetworkSettings.ApplicationIdentifier);
            configuration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            
            _netClient = new NetClient(configuration);
            _netClient.Start();

            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            _netClient.RegisterReceivedCallback(NewMessageArrives);
        }
        
        public void Connect(string userName)
        {
            if (State != NetworkClientState.Disconnected) return;

            _netClient.Connect(HostName, Port, _netClient.CreateMessage(userName));
        }

        private void NewMessageArrives(object state)
        {
            NetIncomingMessage readMessage;
            while ((readMessage = _netClient.ReadMessage()) != null)
            {
                switch (readMessage.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        HandleStatusChanged(readMessage);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void Disconnect()
        {
            if (State == NetworkClientState.Disconnected) return;

            _netClient.Disconnect("Bye!");
            State = NetworkClientState.Disconnected;
        }

        private void HandleStatusChanged(NetIncomingMessage readMessage)
        {
            var netConnectionStatus = (NetConnectionStatus)readMessage.ReadByte();
            switch (netConnectionStatus)
            {
                case NetConnectionStatus.InitiatedConnect:
                    State = NetworkClientState.Connecting;
                    break;
                case NetConnectionStatus.Connected:
                    var remoteHail = readMessage.SenderConnection.RemoteHailMessage;
                    var remoteHailString = remoteHail.ReadString();
                    ClientUniqueId = Guid.Parse(remoteHailString);
                    State = NetworkClientState.Connected;
                    break;
                case NetConnectionStatus.Disconnected:
                    if (State == NetworkClientState.Connecting)
                    {
                        ErrorMessage = readMessage.ReadString();
                        State = NetworkClientState.ConnectionFailed;
                    }
                    else
                    {
                        State = NetworkClientState.Disconnected;
                    }
                    break;
            }
        }
    }
}
