using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.DataModel.GameMode.Map;
using Pokemon3D.UI;
using Pokemon3D.Entities.System;
using Microsoft.Xna.Framework;
using static Pokemon3D.GameCore.GameProvider;
using Pokemon3D.Common.Input;

namespace Pokemon3D.Entities
{
    class World
    {
        private readonly InputSystem _inputSystem;
        private Action _onFinished;
        private readonly NotificationBar _notificationBar;
        private Dictionary<string, Map> _allMaps = new Dictionary<string, Map>(); 

        public Map ActiveMap { get; private set; }
        public Player Player { get; private set; }
        public EntitySystem EntitySystem { get; }

        public World()
        {
            EntitySystem = new EntitySystem();
            _inputSystem = GameInstance.GetService<InputSystem>();
            _notificationBar = GameInstance.GetService<NotificationBar>();
        }

        public void StartNewGameAsync(Action onFinished)
        {
            _onFinished = onFinished;
            GameInstance.ExecuteBackgroundJob(LoadWorldAsync);
        }

        private void LoadWorldAsync()
        {
            var gameMode = GameInstance.GetService<GameModeManager>().ActiveGameMode;
            gameMode.Preload();
            var mapModel = gameMode.LoadMap(gameMode.GameModeInfo.StartMap);
            ActiveMap = new Map(this, mapModel);
            ActiveMap.Load(Vector3.Zero);
            Player = new Player(this);

            //var mapModel2 = gameMode.LoadMap("Route1");
            //var adjacent = new Map(this, mapModel2);
            //adjacent.Load(new Vector3(0,0, -32));

            _onFinished();
        }


        public void ActivateNewEntities()
        {
            EntitySystem.InitializeAllPendingEntities();
        }

        public void Update(GameTime gameTime)
        {
            EntitySystem.Update(gameTime);
            Player.Update(gameTime);

            if (_inputSystem.Keyboard.IsKeyDownOnce(Keys.V))
            {
                if (Player.MovementMode == PlayerMovementMode.GodMode)
                {
                    _notificationBar.PushNotification(NotificationKind.Information, "Disabled God Mode");
                }
                Player.MovementMode = Player.MovementMode == PlayerMovementMode.FirstPerson ? PlayerMovementMode.ThirdPerson : PlayerMovementMode.FirstPerson;
            }

            if (_inputSystem.Keyboard.IsKeyDownOnce(Keys.F10))
            {
                Player.MovementMode = PlayerMovementMode.GodMode;
                _notificationBar.PushNotification(NotificationKind.Information, "Enabled God Mode");
            }
        }
    }
}
