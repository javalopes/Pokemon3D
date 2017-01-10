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

        public event Action<string> OnMessage;

        public GameServer(GameServerConfiguration configuration, IRestClient restClient)
        {
            _configuration = configuration;
            _restClient = restClient;
            _restClient.BaseUrl = new Uri(_configuration.MasterServerUrl);
        }

        public bool Start()
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
                InvokeMessage("Registered successfully");
                return true;
            }

            InvokeMessage($"Registration not successful [{response.StatusCode}]: " + response.ErrorMessage);

            return false;
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
            
        }

        private void InvokeMessage(string message)
        {
            OnMessage?.Invoke(message);
        }
    }
}
