using System;
using System.Net;
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

        public void Start()
        {
            InvokeMessage($"Starting Server {_configuration.Name}...");

            var jsonToSend = $"\"Name\" = \"{_configuration.Name}\", \"IpAddress\" =\"127.0.0.1\"";

            var request = new RestRequest("/api/gameserver/register");
            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            var response = _restClient.Post(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                InvokeMessage("Registered successfully");
            }
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
