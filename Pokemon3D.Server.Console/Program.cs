using System.Threading;
using NLog;

namespace Pokemon3D.Server.Console
{
    internal class Program
    {
        private static readonly ILogger _logger = LogManager.GetLogger("P3D");

        static void Main()
        {
            var configuration = new GameServerConfiguration
            {
                Name = Properties.Settings.Default.ServerName,
                MasterServerUrl = Properties.Settings.Default.MasterServerUrl,
                IsPrivate = Properties.Settings.Default.IsPrivate
            };

            var gameServer = new GameServer(configuration);
            gameServer.OnMessage += OnMessageReceived;

            var cancel = false;
            System.Console.CancelKeyPress += (s, e) =>
            {
                cancel = true;
                e.Cancel = true;
            };

            var startedSuccessfully = gameServer.Start();
            cancel = !startedSuccessfully;

            while (!cancel)
            {
                gameServer.Update();
                Thread.Sleep(1);
            }

            gameServer.Stop();
        }

        private static void OnMessageReceived(string text)
        {
            System.Console.WriteLine(text);
            _logger.Info(text);
        }
    }
}
