using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Pokemon3D.Server.Component
{
    class GameContentComponent : AsyncComponentBase
    {
        private readonly string _zipFilePath;
        private byte[] _data;

        private TcpListener _listener;

        public GameContentComponent(string zipFilePath, IMessageBroker messageBroker) : base(messageBroker)
        {
            _zipFilePath = zipFilePath;
        }

        public override string Name => "Game Mode Content Server";

        protected override void OnStart()
        {
            _data = File.ReadAllBytes(_zipFilePath);

            _listener = new TcpListener(IPAddress.Any, 14555);
            _listener.Start();
        }

        protected override void OnExecute(CancellationToken token)
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
                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    MessageBroker.Notify("An exception occurred during processing a download request: " + ex);
                }
            }
        }
    }
}
