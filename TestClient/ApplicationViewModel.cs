using System;
using System.Windows;
using Pokemon3D.Networking.Client;
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

        public CommandViewModel DisconnectCommand { get; }

        public ApplicationViewModel(IApplicationModel model)
        {
            _model = model;
            ConnectCommand = new CommandViewModel(OnConnect);
            DisconnectCommand = new CommandViewModel(OnDisconnect);
            ServerIp = "localhost";
            Name = "TestUser";
            Port = 14456;
            UpdateCommandStates();
        }

        private void UpdateCommandStates()
        {
            ConnectCommand.IsEnabled = _model.State == NetworkClientState.Disconnected || _model.State == NetworkClientState.ConnectionFailed;
            DisconnectCommand.IsEnabled = _model.State == NetworkClientState.Connected;
        }

        private void OnDisconnect()
        {
            _model.Disconnect();
            UpdateCommandStates();
        }

        private void OnConnect()
        {
            var result = _model.Connect(ServerIp, Port, Name);
            UniqueId = result.Id;
            UpdateCommandStates();

            if (!result.IsConnected)
            {
                MessageBox.Show("Connection failed because: " + result.ErrorMessage, "Error", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
        }
    }
}
