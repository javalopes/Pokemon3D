using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Pokemon3D.Common;
using Pokemon3D.GameModes;
using Pokemon3D.Server.Component;
using Pokemon3D.Server.Management;

namespace Pokemon3D.Server
{
    public class GameServer : GameContext, IMessageBroker
    {
        private readonly object _messageBrokerLockObject = new object();

        private readonly GameServerConfiguration _configuration;
        private GameModeManager _gameModeManager;
        private readonly MasterServerRegistrationClient _masterServerRegistrationClient;
        private readonly ClientRegistrator _clientRegistrator;

        private readonly List<IServerComponent> _components = new List<IServerComponent>();
        public event Action<string> OnMessage;

        public GameServer(GameServerConfiguration configuration)
        {
            _configuration = configuration;
            _masterServerRegistrationClient = new MasterServerRegistrationClient(this);
            _clientRegistrator = new ClientRegistrator(configuration, this);
        }

        public bool Start()
        {
            Notify($"--> Starting Server '{_configuration.Name}' ...");

            try
            {
                if (!RegisterOnMasterServer()) return false;
                if (!PrepareGameMode()) return false;
                if (!StartServerTasks()) return false;
            }
            catch (Exception ex)
            {
                Notify("Unhandled exception occurred: " + ex);
                return false;
            }

            Notify("Server started successfully.");

            return true;
        }

        private bool RegisterOnMasterServer()
        {
            return _masterServerRegistrationClient.Register(_configuration);
        }

        private bool PrepareGameMode()
        {
            _gameModeManager = new GameModeManager();
            var infos = _gameModeManager.GetGameModeInfos();

            if (infos.Length == 0)
            {
                Notify("Did not found the gameMode.");
                return false;
            }

            if (infos.Length > 1)
            {
                Notify("Found more than one gameMode");
                return false;
            }

            var info = infos.Single();
            _gameModeManager.LoadAndSetGameMode(info, this);
            var gameMode = _gameModeManager.ActiveGameMode;
            if (!gameMode.IsValid)
            {
                Notify("Game mode is not valid.");
                return false;
            }

            return true;
        }

        private bool StartServerTasks()
        {
            _components.Add(new GameContentComponent(_gameModeManager.ActiveGameMode, this));
            _components.Add(new CommunicationComponent(this, _clientRegistrator));

            foreach (var component in _components)
            {
                if (component.Start())
                {
                    Notify($"Started '{component.Name}'");
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        
        public void Stop()
        {
            Notify($"Shutting down server '{_configuration.Name}'...");

            _masterServerRegistrationClient.Unregister(_configuration);
            
            foreach (var component in _components)
            {
                Notify($"Stopping '{component.Name}'");
                component.Stop();
            }

            Notify("Done");
        }

        public TService GetService<TService>() where TService : class
        {
            throw new NotImplementedException();
        }

        // ReSharper disable UnassignedGetOnlyAutoProperty
        public Rectangle ScreenBounds { get; }
        // ReSharper restore UnassignedGetOnlyAutoProperty

        public void Notify(string message)
        {
            lock (_messageBrokerLockObject)
            {
                OnMessage?.Invoke(message);
            }
        }

    }
}
