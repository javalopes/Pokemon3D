using Pokemon3D.DataModel.Json.Requests;
using System;
using System.IO;
using System.Linq;
using System.Net;

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

            WebServer ws = new WebServer(SendResponse, "http://localhost:8080/" + SERVER_API + "/");
            ws.Run();
            Console.ReadLine();
            ws.Stop();
        }
        private static string StartupPath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        public static string SendResponse(HttpListenerRequest request)
        {
            string requestUrl = request.Url.PathAndQuery;
            requestUrl = requestUrl.TrimStart('/');
            requestUrl = requestUrl.Remove(0, SERVER_API.Length);
            requestUrl = requestUrl.TrimStart('/');

            Console.WriteLine("Request received to: " + requestUrl);

            string path = Path.Combine(StartupPath, "GameModes", _gameMode, requestUrl);

            if (Directory.Exists(path))
            {
                var dataModels = Directory.GetFiles(path).Select(file => new FileContentModel()
                {
                    FileName = file,
                    FileContent = File.ReadAllText(file)
                }).ToArray();
                string returnStr = "[";
                for (int i = 0; i < dataModels.Length; i++)
                {
                    returnStr += dataModels[i];
                    if (i < dataModels.Length - 1)
                    {
                        returnStr += ",";
                    }
                }
                returnStr += "]";
                return returnStr;
            }
            else if (File.Exists(path))
            {
                string fileContent = File.ReadAllText(path);
                var dataModel = new FileContentModel()
                {
                    FileName = requestUrl,
                    FileContent = fileContent
                };
                string returnStr = "[" + dataModel.ToString() + "]";
                return returnStr;
            }
            else
            {
                return "ERROR! File/Directory not found!";
            }
        }
    }
}
