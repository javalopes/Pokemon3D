using System;
using System.Net;
using Pokemon3D.Master.Server.DataContracts;
using RestSharp;

namespace Pokemon3D.Server
{
    public class GameServer
    {
        private readonly GameServerConfiguration _configuration;
        private readonly IRestClient _restClient;
        public int _gameServerId;

        public event Action<string> OnMessage;

        public GameServer(GameServerConfiguration configuration, IRestClient restClient)
        {
            _configuration = configuration;
            _restClient = restClient;
            _restClient.BaseUrl = new Uri(_configuration.MasterServerUrl);
            _gameServerId = -1;
        }

        public bool Start()
        {
            var result = false;
            try
            {
                InvokeMessage($"Starting Server {_configuration.Name}...");

                var gameServerData = new GameServerRegistrationData
                {
                    Name = _configuration.Name,
                    IpAddress = "127.0.0.1"
                };

                var response = SendPostRequest("/api/gameserver/register", gameServerData);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _gameServerId = int.Parse(response.Content);
                    InvokeMessage($"Registered successfully with id = {_gameServerId}");
                    result = true;
                }
                else
                {
                    InvokeMessage($"Registration not successful [{response.StatusCode}]: " + response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                InvokeMessage("Unhandled exception occurred: " + ex);
            }

            return result;
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
            if (_gameServerId != -1)
            {
                SendPostRequest("/api/gameserver/unregister", _gameServerId);
            }
        }

        private void InvokeMessage(string message)
        {
            OnMessage?.Invoke(message);
        }
    }
}
