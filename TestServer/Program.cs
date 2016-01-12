using Pokemon3D.DataModel.Requests;
using Pokemon3D.Server;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace TestServer
{
    class Program
    {
        const string SERVER_API = "api";

        private static string _gameMode;

        static void Main(string[] args)
        {
            Console.WriteLine("Pokémon3D Test REST server.");

            Console.Write("Input path to GameMode: ");
            _gameMode = Console.ReadLine();
            Console.WriteLine("Path set to \"" + _gameMode + "\"");

            HttpServer ws = new HttpServer(_gameMode, "localhost", "8080");
            ws.Run();
            Console.ReadLine();
            ws.Stop();
        }
    }
}
