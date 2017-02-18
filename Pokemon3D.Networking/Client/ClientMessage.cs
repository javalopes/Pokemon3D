using System;
using Lidgren.Network;

namespace Pokemon3D.Networking.Client
{
    public abstract class ClientMessage : Message
    {
        public Guid ClientIdentifier { get; private set; }

        protected ClientMessage(Guid identifier)
        {
            ClientIdentifier = identifier;
        }

        protected ClientMessage()
        {
            
        }

        public override void Read(NetIncomingMessage message)
        {
            ClientIdentifier = Guid.Parse(message.ReadString());
        }

        public override void Write(NetOutgoingMessage message)
        {
            message.Write(ClientIdentifier.ToString());
        }
    }
}