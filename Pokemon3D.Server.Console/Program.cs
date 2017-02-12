using System.Threading;
using NLog;

namespace Pokemon3D.Server.Console
{
    internal class Program
    {
        private static readonly ILogger Logger = LogManager.GetLogger("P3D");

        private static void Main()
        {
            var configuration = new GameServerConfiguration
            {
                Name = Properties.Settings.Default.ServerName,
                MasterServerUrl = Properties.Settings.Default.MasterServerUrl,
                IsPrivate = Properties.Settings.Default.IsPrivate,
                MaxPlayerCount = Properties.Settings.Default.MaxPlayerCount
            };

            var gameServer = new GameServer(configuration);
            gameServer.OnMessage += OnMessageReceived;

            var running = gameServer.Start();
            System.Console.CancelKeyPress += (s, e) =>
            {
                running = false;
                e.Cancel = true;
            };
            
            while (running) Thread.Sleep(1);

            gameServer.Stop();
        }

        private static void OnMessageReceived(string text)
        {
            System.Console.WriteLine(text);
            Logger.Info(text);
        }
    }
}
