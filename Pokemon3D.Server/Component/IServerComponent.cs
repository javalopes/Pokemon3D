using Pokemon3D.Networking;

namespace Pokemon3D.Server.Component
{
    interface IServerComponent
    {
        string Name { get; }

        bool Start();

        void Stop();

        bool HandleMessage(ClientMessage clientMessage);
    }
}