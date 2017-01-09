using System.Threading;
using RestSharp;

namespace Pokemon3D.Server.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient();
            var configuration = new GameServerConfiguration
            {
                Name = "Peters Server",
                MasterServerUrl = "http://localhost:15710",
                IsPrivate = false
            };

            var gameServer = new GameServer(configuration, client);
            gameServer.OnMessage += System.Console.WriteLine;

            bool cancel = false;
            System.Console.CancelKeyPress += (s,e) => cancel = true;

            gameServer.Start();

            while (!cancel)
            {
                Thread.Sleep(1);
            }

            gameServer.Stop();
        }
    }
}
