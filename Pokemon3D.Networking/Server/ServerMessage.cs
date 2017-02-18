using System;
using Lidgren.Network;

namespace Pokemon3D.Networking.Server
{
    public abstract class ServerMessage : Message
    {
        public Guid ClientUniqueId { get; private set; }

        protected ServerMessage(Guid clientIdentiier)
        {
            ClientUniqueId = clientIdentiier;
        }

        protected ServerMessage()
        {
        }
        
        public override void Read(NetIncomingMessage message)
        {
            ClientUniqueId = Guid.Parse(message.ReadString());
        }

        public override void Write(NetOutgoingMessage message)
        {
            message.Write(ClientUniqueId.ToString());
        }
    }
}
