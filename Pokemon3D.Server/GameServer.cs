﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using Ionic.Zip;
using Microsoft.Xna.Framework;
using Pokemon3D.Common;
using Pokemon3D.DataModel.Multiplayer;
using Pokemon3D.GameModes;
using RestSharp;

namespace Pokemon3D.Server
{
    public class GameServer : GameContext
    {
        private readonly GameServerConfiguration _configuration;
        private int _gameServerId;
        private GameModeManager _gameModeManager;
        private readonly RestClient _restClient;

        public event Action<string> OnMessage;

        public GameServer(GameServerConfiguration configuration)
        {
            _configuration = configuration;
            _gameServerId = -1;
            _restClient = new RestClient();
        }

        public bool Start()
        {
            InvokeMessage($"--> Starting Server '{_configuration.Name}' ...");

            try
            {
                if (!RegisterOnMasterServer()) return false;
                if (!PrepareGameMode()) return false;
            }
            catch (Exception ex)
            {
                InvokeMessage("Unhandled exception occurred: " + ex);
                return false;
            }

            InvokeMessage("Server started successfully.");

            return true;
        }

        private bool RegisterOnMasterServer()
        {
            if (string.IsNullOrEmpty(_configuration.MasterServerUrl))
            {
                InvokeMessage("No master server url declared, no registration is done.");
                return true;
            }
            
            _restClient.BaseUrl = new Uri(_configuration.MasterServerUrl);

            var gameServerData = new GameServerRegistrationModel
            {
                Name = _configuration.Name,
                IpAddress = "127.0.0.1"
            };

            var response = SendPostRequest("/api/gameserver/register", gameServerData);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                _gameServerId = int.Parse(response.Content);
                InvokeMessage($"Registered successfully with id = {_gameServerId}");
                return true;
            }

            InvokeMessage($"Registration not successful  with status code {response.StatusCode}: {response.ErrorMessage}");
            return false;
        }

        private bool PrepareGameMode()
        {
            InvokeMessage("Preparing Game Mode...");

            _gameModeManager = new GameModeManager();
            var infos = _gameModeManager.GetGameModeInfos();

            if (infos.Length == 0)
            {
                InvokeMessage("Did not found the gameMode.");
                return false;
            }

            if (infos.Length > 1)
            {
                InvokeMessage("Found more than one gameMode");
                return false;
            }

            var info = infos.Single();
            _gameModeManager.LoadAndSetGameMode(info, this);
            var gameMode = _gameModeManager.ActiveGameMode;
            if (!gameMode.IsValid)
            {
                InvokeMessage("Game mode is not valid.");
                return false;
            }

            var rootFolderName = Path.GetFileName(gameMode.GameModeInfo.DirectoryPath);
            var parentFolder = Path.GetDirectoryName(gameMode.GameModeInfo.DirectoryPath);
            var targetZipFilePath = Path.Combine(parentFolder, rootFolderName + ".zip");

            if (File.Exists(targetZipFilePath))
            {
                File.Delete(targetZipFilePath);
                InvokeMessage("Existing Game Mode content package deleted");
            }

            using (var zipFile = new ZipFile(targetZipFilePath))
            {
                zipFile.AddDirectory(Path.Combine(gameMode.GameModeInfo.DirectoryPath, "Content"));
                zipFile.Comment = gameMode.CalculateChecksum().ToString();
                zipFile.Save();
            }

            InvokeMessage("Game mode has been zipped and saved with checksum");

            return true;
        }

        public void Update()
        {
            
        }
        
        private IRestResponse SendPostRequest(string requestUriPart, object toSend)
        {
            var request = new RestRequest(requestUriPart);
            var jsonToSend = request.JsonSerializer.Serialize(toSend);

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            return _restClient.Post(request);
        }

        public void Stop()
        {
            InvokeMessage($"Shutting down server '{_configuration.Name}'...");
            if (_gameServerId != -1 && !string.IsNullOrEmpty(_configuration.MasterServerUrl))
            {
                SendPostRequest("/api/gameserver/unregister", _gameServerId);
            }
            InvokeMessage("Done");
        }

        private void InvokeMessage(string message)
        {
            OnMessage?.Invoke(message);
        }

        public TService GetService<TService>() where TService : class
        {
            throw new NotImplementedException();
        }

        public Rectangle ScreenBounds { get; }
    }
}
