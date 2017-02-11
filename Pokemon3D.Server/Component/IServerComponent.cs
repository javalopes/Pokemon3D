namespace Pokemon3D.Server.Component
{
    interface IServerComponent
    {
        string Name { get; }
        bool Start();
        void Stop();
    }
}