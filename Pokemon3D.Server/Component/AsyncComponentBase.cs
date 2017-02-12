using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon3D.Server.Component
{
    abstract class AsyncComponentBase : IServerComponent
    {
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

        protected abstract void OnStart();

        protected abstract void OnExecute(CancellationToken token);
    }
}