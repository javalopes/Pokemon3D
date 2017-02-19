using System;
using Lidgren.Network;

namespace Pokemon3D.Server.Management
{
    public interface IClientRegistrator
    {
        bool RegisterClient(Player player);

        void UnregisterClient(Player player);

        Player GetPlayer(Guid uniqueId);
        
        Player GetPlayer(NetConnection senderConnection);
    }
}