using System.Threading;
using Pokemon3D.Networking.Client;

namespace TestClient
{
    public class ApplicationModel : IApplicationModel
    {
        private GameNetworkClient _client;

        public NetworkClientState State => _client?.State ?? NetworkClientState.Disconnected;

        public ConnectionResult Connect(string serverIp, int port, string name)
        {
            var connectionResult = new ConnectionResult();
            _client = new GameNetworkClient(serverIp, port);

            _client.Connect(name);
            
            while (_client.State != NetworkClientState.Connected && _client.State != NetworkClientState.ConnectionFailed)
            {
                Thread.Sleep(1);
            }

            if (_client.State == NetworkClientState.Connected)
            {
                connectionResult.IsConnected = true;
                connectionResult.Id = _client.ClientUniqueId;
            }
            else 
            {
                connectionResult.IsConnected = false;
                connectionResult.ErrorMessage = _client.ErrorMessage;
            }

            return connectionResult;
        }

        public void Disconnect()
        {
            _client.Disconnect();
        }
    }
}