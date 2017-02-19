using System;
using Lidgren.Network;
using Pokemon3D.Networking;

namespace TestClient
{
    public class ApplicationModel : IApplicationModel
    {
        private NetClient _netClient;

        public Guid Connect(string serverIp, int port, string name)
        {
            var configuration = new NetPeerConfiguration(NetworkSettings.ApplicationIdentifier);
            configuration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            _netClient = new NetClient(configuration);
            _netClient.Start();

            _netClient.Connect(serverIp, port, _netClient.CreateMessage(name));

            while (true)
            {
                var readMessage = _netClient.ReadMessage();
                if (readMessage == null) continue;

                switch (readMessage.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        var guid = HandleStatusChanged(readMessage);
                        if (guid != Guid.Empty) return guid;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private Guid HandleStatusChanged(NetIncomingMessage readMessage)
        {
            var netConnectionStatus = (NetConnectionStatus)readMessage.ReadByte();
            switch (netConnectionStatus)
            {
                case NetConnectionStatus.Connected:
                    var remoteHail = readMessage.SenderConnection.RemoteHailMessage;
                    var remoteHailString = remoteHail.ReadString();
                    return Guid.Parse(remoteHailString);
            }
            return Guid.Empty;
        }
    }
}