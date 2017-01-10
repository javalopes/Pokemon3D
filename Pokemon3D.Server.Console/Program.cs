using System.Threading;
using RestSharp;

namespace Pokemon3D.Server.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient();
            var configuration = new GameServerConfiguration
            {
                Name = Properties.Settings.Default.ServerName,
                MasterServerUrl = Properties.Settings.Default.MasterServerUrl,
                IsPrivate = Properties.Settings.Default.IsPrivate
            };

            var gameServer = new GameServer(configuration, client);
            gameServer.OnMessage += System.Console.WriteLine;

            var cancel = false;
            System.Console.CancelKeyPress += (s,e) => cancel = true;

            var startedSuccessfully = gameServer.Start();
            cancel = !startedSuccessfully;

            while (!cancel)
            {
                Thread.Sleep(1);
            }

            gameServer.Stop();

            if (!startedSuccessfully)
            {
                System.Console.WriteLine("Press enter to quit...");
                System.Console.ReadLine();
            }
        }
    }
}
