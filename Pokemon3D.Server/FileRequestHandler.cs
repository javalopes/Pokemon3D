using System;
using System.Linq;
using System.Text;
using System.IO;
using Pokemon3D.DataModel;
using Pokemon3D.DataModel.Requests;

namespace Pokemon3D.Server
{
    class FileRequestHandler
    {
        private readonly string _gameModeFolder;

        private static string StartupPath => AppDomain.CurrentDomain.BaseDirectory;

        public FileRequestHandler(string gameModeFolder)
        {
            _gameModeFolder = Path.Combine(StartupPath, "GameModes", gameModeFolder);
        }

        public byte[] HandleRequest(string requestPath)
        {
            string path = Path.Combine(_gameModeFolder, requestPath);

            // check if it's a directory request:
            if (Directory.Exists(path))
            {
                var dataModels = Directory.GetFiles(path).Select(file => new FileContentModel()
                {
                    FileName = Path.Combine(requestPath, Path.GetFileName(file) ?? ""),
                    FileContent = File.ReadAllText(file)
                }).ToArray();
                string returnStr = "[";
                for (int i = 0; i < dataModels.Length; i++)
                {
                    returnStr += dataModels[i].ToString(DataType.Json);
                    if (i < dataModels.Length - 1)
                    {
                        returnStr += ",";
                    }
                }
                returnStr += "]";
                return Encoding.UTF8.GetBytes(returnStr);
            }
            // else, check if it's a request to a single file:
            else if (File.Exists(path))
            {
                string fileContent = File.ReadAllText(path);
                var dataModel = new FileContentModel()
                {
                    FileName = requestPath,
                    FileContent = fileContent
                };
                string returnStr = "[" + dataModel.ToString(DataType.Json) + "]";
                return Encoding.UTF8.GetBytes(returnStr);
            }

            // Return empty byte array:
            return new byte[] { };
        }
    }
}
