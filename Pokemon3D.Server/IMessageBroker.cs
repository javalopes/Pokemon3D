namespace Pokemon3D.Server
{
    public interface IMessageBroker
    {
        void Notify(string message);
    }
}