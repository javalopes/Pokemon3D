using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Pokemon3D.Networking
{
    public static class NetPeerExtensions
    {
        public static NetSendResult SendMessage(this NetPeer peer, NetConnection receipent, Message message, NetDeliveryMethod deliveryMethod = NetDeliveryMethod.ReliableOrdered)
        {
            var peerMessage = peer.CreateMessage();
            peerMessage.Write((int)message.MessageType);
            message.Write(peerMessage);

            return peer.SendMessage(peerMessage, receipent, deliveryMethod);
        }
    }
}
