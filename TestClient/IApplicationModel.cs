using System;

namespace TestClient
{
    public interface IApplicationModel
    {
        Guid Connect(string serverIp, int port, string name);
    }
}