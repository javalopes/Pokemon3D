using System;
using Pokemon3D.Networking.Client;

namespace TestClient
{
    public interface IApplicationModel
    {
        NetworkClientState State { get; }

        Guid Connect(string serverIp, int port, string name);

        void Disconnect();
    }
}