using System;
using System.Collections.Generic;
using Lidgren.Network;
using Pokemon3D.Networking;
using Pokemon3D.Networking.Client;
using Pokemon3D.Networking.Server;

namespace Pokemon3D.Server.Management
{
    class NetworkCommunication
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IClientRegistrator _registrator;
        private readonly NetServer _server;
        private readonly List<ClientMessage> _messageBuffer = new List<ClientMessage>();

        public NetworkCommunication(IMessageBroker messageBroker, IClientRegistrator registrator, int port) 
        {
            if (messageBroker == null) throw new ArgumentNullException(nameof(messageBroker));
            _messageBroker = messageBroker;
            _registrator = registrator;
            var configuration = new NetPeerConfiguration(NetworkSettings.ApplicationIdentifier)
            {
                Port = port
            };

            configuration.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            _server = new NetServer(configuration);
        }

        public void StartListening()
        {
            _server.Start();
        }

        public void Shutdown()
        {
            _server.Shutdown("Shutdown from server");
        }

        public ClientMessage[] ReadMessages()
        {
            _messageBuffer.Clear();

            NetIncomingMessage msg;
            while ((msg = _server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        OnConnectionApproval(msg);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                        _messageBroker.Notify("Network VerboseDebugMsg: " + msg.ReadString());
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        _messageBroker.Notify("Network DebugMsg: " + msg.ReadString());
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        _messageBroker.Notify("Network WarningMsg: " + msg.ReadString());
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        _messageBroker.Notify("Network ErrorMsg: " + msg.ReadString());
                        break;
                    case NetIncomingMessageType.Data:
                        _messageBuffer.Add((ClientMessage)Message.ReadMessage(msg));
                        break;
                    default:
                        _messageBroker.Notify("Unhandled type: " + msg.MessageType);
                        break;
                }
                _server.Recycle(msg);
            }

            return _messageBuffer.ToArray();
        }
        
        private void OnConnectionApproval(NetIncomingMessage incomingMessage)
        {
            var player = new Player(incomingMessage.ReadString(), incomingMessage.SenderConnection);
            if (_registrator.RegisterClient(player))
            {
                var msg = _server.CreateMessage();  
                msg.Write(player.UniqueIdentifier.ToString());
                incomingMessage.SenderConnection.Approve(msg);
            }
            else
            {
                incomingMessage.SenderConnection.Deny("Server full.");
            }
        }

        public void SendMessages(IEnumerable<ServerMessage> allMessages)
        {
            foreach (var serverMessage in allMessages)
            {
                var player = _registrator.GetPlayer(serverMessage.ClientUniqueId);
                _server.SendMessage(player.Connection, serverMessage);
            }
        }
    }
}
