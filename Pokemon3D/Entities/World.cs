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
            GameInstance.ExecuteBackgroundJob(LoadWorldAsync, onFinished);
        }

        private void LoadWorldAsync()
        {
            var gameMode = GameInstance.GetService<GameModeManager>().ActiveGameMode;
            gameMode.Preload();
            
            Player = new Player(this);
            ActiveMap = LoadMap(gameMode.GameModeInfo.StartMap, Vector3.Zero, true);
            EntitySystem.InitializeAllPendingEntities();
        }

        public void AddMap(string id, double x, double y, double z)
        {
            Map existingMap;
            if (_allMaps.TryGetValue(id, out existingMap))
            {
                existingMap.Activate();
            }
            else
            {
                var map = LoadMap(id, new Vector3((float)x, (float)y, (float)z));
                _allMaps.Add(id, map);
                EntitySystem.InitializeAllPendingEntities();
            }
        }

        private Map LoadMap(string id, Vector3 position, bool loadOffsets = true)
        {
            var gameMode = GameInstance.GetService<GameModeManager>().ActiveGameMode;
            var mapModel = gameMode.LoadMap(id);
            var map = new Map(this, mapModel);
            map.Load(position);

            if (loadOffsets && map.Model.OffsetMaps != null)
            {
                foreach(var offsetmap in map.Model.OffsetMaps)
                {
                    LoadMap(offsetmap.MapFile, position + offsetmap.Offset.GetVector3(), false);
                }
            }

            EntitySystem.InitializeAllPendingEntities();
            return map;
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
