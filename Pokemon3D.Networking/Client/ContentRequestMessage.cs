using Lidgren.Network;

namespace Pokemon3D.Networking
{
    public class ContentRequestMessage : ClientMessage
    {
        public long Checksum { get; private set; }

        public ContentRequestMessage(long checksum)
        {
            Checksum = checksum;
        }

        internal ContentRequestMessage()
        {
        }

        public override MessageType MessageType { get; } = MessageType.ContentRequestMessage;

        public override void Read(NetIncomingMessage message)
        {
            base.Read(message);
            Checksum = message.ReadInt64();
        }

        public override void Write(NetOutgoingMessage message)
        {
            base.Write(message);
            message.Write(Checksum);
        }
    }
}