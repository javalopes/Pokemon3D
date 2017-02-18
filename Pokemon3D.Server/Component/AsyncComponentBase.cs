using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Pokemon3D.Networking.Client;
using Pokemon3D.Networking.Server;
using Pokemon3D.Server.Management;

namespace Pokemon3D.Server.Component
{
    abstract class AsyncComponentBase : IServerComponent
    {
        private readonly List<ServerMessage> _serverMessages = new List<ServerMessage>();
        private readonly Queue<ClientMessage> _clientMessages = new Queue<ClientMessage>();
        private CancellationTokenSource _cancelSource;
        private Task _task;

        protected IMessageBroker MessageBroker { get; }

        public abstract string Name { get; }

        protected AsyncComponentBase(IMessageBroker messageBroker)
        {
            MessageBroker = messageBroker;
        }

        public bool Start()
        {
            try
            {
                OnStart();

                _cancelSource = new CancellationTokenSource();
                _task = Task.Factory.StartNew(() => OnExecute(_cancelSource.Token), _cancelSource.Token);
            }
            catch (Exception ex)
            {
                MessageBroker.Notify($"Starting task '{Name}' failed: " + ex);
                return false;
            }

            return true;
        }

        public void Stop()
        {
            _cancelSource.Cancel();
            if (!_task.Wait(TimeSpan.FromSeconds(10)))
            {
                MessageBroker.Notify($"Stopping Task '{Name}' was not possible withing 10 seconds");
            }
        }

        public virtual bool HandleMessage(ClientMessage clientMessage)
        {
            if (IsHandledMessageType(clientMessage))
            {
                lock (_clientMessages)
                {
                    _clientMessages.Enqueue(clientMessage);
                }
                return true;
            }
            return false;
        }

        public ServerMessage[] GetAndClearServerMessages()
        {
            lock (_serverMessages)
            {
                var result = _serverMessages.ToArray();
                _serverMessages.Clear();
                return result;
            }
        }

        protected ClientMessage DequeueClientMessage()
        {
            lock (_clientMessages)
            {
                if (_clientMessages.Count > 0) return _clientMessages.Dequeue();
            }

            return null;
        }

        protected void EnqueueSendingMessage(ServerMessage message)
        {
            lock (_serverMessages)
            {
                _serverMessages.Add(message);
            }
        }

        protected abstract bool IsHandledMessageType(ClientMessage clientMessage);

        protected abstract void OnStart();

        protected abstract void OnExecute(CancellationToken token);
    }
}