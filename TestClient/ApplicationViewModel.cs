using System;
using TestClient.Framework;

namespace TestClient
{
    public class ApplicationViewModel : ViewModel
    {
        private readonly IApplicationModel _model;
        private string _serverIp;
        private int _port;
        private string _name;
        private Guid _uniqueId;

        public string ServerIp
        {
            get { return _serverIp; }
            set { SetProperty(ref _serverIp, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public int Port
        {
            get { return _port; }
            set { SetProperty(ref _port, value); }
        }

        public Guid UniqueId
        {
            get { return _uniqueId; }
            set { SetProperty(ref _uniqueId, value); }
        }

        public CommandViewModel ConnectCommand { get; }

        public ApplicationViewModel(IApplicationModel model)
        {
            _model = model;
            ConnectCommand = new CommandViewModel(OnConnect);
            ServerIp = "localhost";
            Name = "TestUser";
            Port = 14456;
        }

        private void OnConnect()
        {
            UniqueId = _model.Connect(ServerIp, Port, Name);
        }
    }
}
