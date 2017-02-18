using System;
using Lidgren.Network;

namespace Pokemon3D.Networking.Server
{
    public class ContentResponseMessage : ServerMessage
    {
        public bool ChecksumCorrect { get; private set; }

        public ContentResponseMessage(Guid uniqueId, bool checksumCorrect) : base(uniqueId)
        {
            ChecksumCorrect = checksumCorrect;
        }

        public ContentResponseMessage()
        {
            
        }

        public override MessageType MessageType { get; } = MessageType.ContentResponseMessage;

        public override void Read(NetIncomingMessage message)
        {
            base.Read(message);
            ChecksumCorrect = message.ReadBoolean();
        }

        public override void Write(NetOutgoingMessage message)
        {
            base.Write(message);
            message.Write(ChecksumCorrect);
        }
    }
}
