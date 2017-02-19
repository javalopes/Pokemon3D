using System;
using Pokemon3D.Networking.Client;

namespace TestClient
{
    public class ApplicationModel : IApplicationModel
    {
        private GameNetworkClient _client;

        public Guid Connect(string serverIp, int port, string name)
        {
            _client = new GameNetworkClient(serverIp, port);

            _client.Connect(name);
            
            while (_client.State != NetworkClientState.Connected)
            {
                _client.ProcessMessages();
            }

            return _client.ClientUniqueId;
        }
    }
}