using System;
using System.Net;
using Pokemon3D.DataModel.Multiplayer;
using RestSharp;

namespace Pokemon3D.Server.Management
{
    class MasterServerRegistrationClient
    {
        private readonly IMessageBroker _messageBroker;
        private readonly RestClient _restClient;

        public int GameServerId { get; private set; }

        public MasterServerRegistrationClient(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
            _restClient = new RestClient();
            GameServerId = -1;
        }

        public bool Register(GameServerConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration.MasterServerUrl))
            {
                _messageBroker.Notify("No master server url declared, no registration is done.");
                return true;
            }

            _restClient.BaseUrl = new Uri(configuration.MasterServerUrl);
            
            var gameServerData = new GameServerRegistrationModel
            {
                Name = configuration.Name,
                IpAddress = "127.0.0.1"
            };

            var response = SendPostRequest("/api/gameserver/register", gameServerData);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                GameServerId = int.Parse(response.Content);
                _messageBroker.Notify($"Registered successfully with id = {GameServerId}");
                return true;
            }

            _messageBroker.Notify($"Registration not successful  with status code {response.StatusCode}: {response.ErrorMessage}");
            return false;
        }

        public void Unregister(GameServerConfiguration configuration)
        {
            if (GameServerId != -1 && !string.IsNullOrEmpty(configuration.MasterServerUrl))
            {
                SendPostRequest("/api/gameserver/unregister", GameServerId);
            }
        }

        private IRestResponse SendPostRequest(string requestUriPart, object toSend)
        {
            var request = new RestRequest(requestUriPart);
            var jsonToSend = request.JsonSerializer.Serialize(toSend);

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            return _restClient.Post(request);
        }
    }
}
