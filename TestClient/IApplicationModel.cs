using System;
using Pokemon3D.Networking.Client;

namespace TestClient
{
    public class ConnectionResult
    {
        public bool IsConnected { get; set; }

        public Guid Id { get; set; }

        public string ErrorMessage { get; set; }
    }

    public interface IApplicationModel
    {
        NetworkClientState State { get; }

        ConnectionResult Connect(string serverIp, int port, string name);

        void Disconnect();
    }
}