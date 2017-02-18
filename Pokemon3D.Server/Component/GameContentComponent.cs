using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Ionic.Zip;
using Pokemon3D.GameModes;
using Pokemon3D.Networking;
using Pokemon3D.Networking.Client;
using Pokemon3D.Networking.Server;
using Pokemon3D.Server.Management;

namespace Pokemon3D.Server.Component
{
    class GameContentComponent : AsyncComponentBase
    {
        private readonly GameMode _gameMode;
        private readonly IClientRegistrator _registrator;
        private readonly int _portNumber;
        private readonly string _gameModeRootPath;
        private byte[] _data;
        
        private TcpListener _listener;
        private long _checkSum;

        public GameContentComponent(GameMode gameMode, IMessageBroker messageBroker, IClientRegistrator registrator, int portNumber) 
            : base(messageBroker)
        {
            _gameMode = gameMode;
            _registrator = registrator;
            _portNumber = portNumber;
            _gameModeRootPath = _gameMode.GameModeInfo.DirectoryPath;
        }

        public override string Name => "Game Mode Content Server";

        protected override bool IsHandledMessageType(ClientMessage clientMessage)
        {
            return clientMessage is ContentRequestMessage;
        }

        protected override void OnStart()
        {
            var rootFolderName = Path.GetFileName(_gameModeRootPath);
            var parentFolder = Path.GetDirectoryName(_gameModeRootPath) ?? "";
            var contentFilePath = Path.Combine(parentFolder, rootFolderName + ".zip");

            if (File.Exists(contentFilePath))
            {
                File.Delete(contentFilePath);
                MessageBroker.Notify("Existing Game Mode content package deleted");
            }

            using (var zipFile = new ZipFile(contentFilePath))
            {
                zipFile.AddDirectory(Path.Combine(_gameModeRootPath, "Content"));
                _checkSum = _gameMode.CalculateChecksum();
                zipFile.Comment = _checkSum.ToString();
                zipFile.Save();
            }
            _data = File.ReadAllBytes(contentFilePath);

            _listener = new TcpListener(IPAddress.Any, _portNumber);
            _listener.Start();
        }

        protected override void OnExecute(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested) break;

                try
                {
                    var request = DequeueClientMessage() as ContentRequestMessage;
                    if (request != null)
                    {
                        EnqueueSendingMessage(new ContentResponseMessage(request.ClientIdentifier, request.Checksum == _checkSum));
                    }
                    
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
