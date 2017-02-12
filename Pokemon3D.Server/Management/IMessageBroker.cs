using System;

namespace Pokemon3D.Server.Management
{
    public interface IMessageBroker
    {
        void Notify(string message);
    }

    public interface IClientRegistrator
    {
        bool RegisterClient(Player player);

        void UnregisterClient(Player player);

        Player GetPlayer(Guid uniqueId);
    }
}