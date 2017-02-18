using Lidgren.Network;

namespace Pokemon3D.Networking.Server
{
    public class ContentResponseMessage : Message
    {
        public bool ChecksumCorrect { get; private set; }

        public ContentResponseMessage(bool checksumCorrect)
        {
            ChecksumCorrect = checksumCorrect;
        }

        public ContentResponseMessage()
        {
            
        }

        public override MessageType MessageType { get; } = MessageType.ContentResponseMessage;

        public override void Read(NetIncomingMessage message)
        {
            ChecksumCorrect = message.ReadBoolean();
        }

        public override void Write(NetOutgoingMessage message)
        {
            message.Write(ChecksumCorrect);
        }
    }
}
