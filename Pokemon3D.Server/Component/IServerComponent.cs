using Pokemon3D.Networking.Client;
using Pokemon3D.Networking.Server;

namespace Pokemon3D.Server.Component
{
    interface IServerComponent
    {
        string Name { get; }

        bool Start();

        void Stop();

        bool HandleMessage(ClientMessage clientMessage);

        ServerMessage[] GetAndClearServerMessages();
    }
}