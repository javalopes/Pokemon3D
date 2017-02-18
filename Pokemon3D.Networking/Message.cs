using System;
using Lidgren.Network;

namespace Pokemon3D.Networking
{
    public abstract class Message
    {
        public abstract MessageType MessageType { get; }

        public abstract void Read(NetIncomingMessage message);

        public abstract void Write(NetOutgoingMessage message);

        public static Message ReadMessage(NetIncomingMessage incomingMessage)
        {
            var messageType = (MessageType)incomingMessage.ReadInt32();

            Message message;
            switch (messageType)
            {
                case MessageType.ContentRequestMessage:
                    message = new ContentRequestMessage();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            message.Read(incomingMessage);

            return message;
        }
    }
}