using System;
using System.Threading;
using Pokemon3D.Networking.Client;

namespace TestClient
{
    public class ApplicationModel : IApplicationModel
    {
        private GameNetworkClient _client;

        public NetworkClientState State => _client?.State ?? NetworkClientState.Disconnected;

        public Guid Connect(string serverIp, int port, string name)
        {
            _client = new GameNetworkClient(serverIp, port);

            _client.Connect(name);
            
            while (_client.State != NetworkClientState.Connected)
            {
                Thread.Sleep(1);
            }

            return _client.ClientUniqueId;
        }

        public void Disconnect()
        {
            _client.Disconnect();
        }
    }
}