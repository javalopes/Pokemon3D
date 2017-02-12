using System;

namespace Pokemon3D.Server.Management
{
    public interface IClientRegistrator
    {
        bool RegisterClient(Player player);

        void UnregisterClient(Player player);

        Player GetPlayer(Guid uniqueId);
    }
}