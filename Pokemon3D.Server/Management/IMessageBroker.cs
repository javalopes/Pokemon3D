namespace Pokemon3D.Server.Management
{
    public interface IMessageBroker
    {
        void Notify(string message);
    }
}