using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon3D.Server.Component
{
    class GameContentComponent : IServerComponent
    {
        private readonly string _zipFilePath;
        private readonly IMessageBroker _messageBroker;
        private byte[] _data;

        private CancellationTokenSource _cancelSource;
        private Task _task;
        private TcpListener _listener;

        public GameContentComponent(string zipFilePath, IMessageBroker messageBroker)
        {
            _zipFilePath = zipFilePath;
            _messageBroker = messageBroker;
        }

        public bool Start()
        {
            try
            {
                _data = File.ReadAllBytes(_zipFilePath);

                _listener = new TcpListener(IPAddress.Any, 14555);
                _listener.Start();

                _cancelSource = new CancellationTokenSource();
                _task = Task.Factory.StartNew(() => ListenToFileRequests(_cancelSource.Token), _cancelSource.Token);
            }
            catch (Exception ex)
            {
                _messageBroker.Notify("Starting Content Download task failed: " + ex);
                return false;
            }

            return true;
        }

        public void Stop()
        {
            _cancelSource.Cancel();
            if (!_task.Wait(TimeSpan.FromSeconds(10)))
            {
                _messageBroker.Notify("Stopping Content Download task was not possible withing 10 seconds");
            }
        }

        private void ListenToFileRequests(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested) break;

                try
                {
                    if (_listener.Pending())
                    {
                        var client = _listener.AcceptTcpClient();

                        var stream = client.GetStream();
                        stream.Write(_data, 0, _data.Length);
                    }
                }
                catch (Exception ex)
                {
                    _messageBroker.Notify("An exception occurred during processing a download request: " + ex);
                }
            }
        }
    }
}
