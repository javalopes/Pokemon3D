using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.UI;
using Pokemon3D.Entities.System;
using Microsoft.Xna.Framework;
using static Pokemon3D.GameCore.GameProvider;
using Pokemon3D.Common.Input;
using Pokemon3D.GameModes;

namespace Pokemon3D.Entities
{
    internal class World
    {
        private readonly InputSystem _inputSystem;
        private readonly NotificationBar _notificationBar;
        private readonly Dictionary<string, Map> _allMaps = new Dictionary<string, Map>();

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
            ActivateMapsWithOffsets(gameMode.GameModeInfo.StartMap, Vector3.Zero);
            EntitySystem.InitializeAllPendingEntities();
        }

        private void ActivateMapsWithOffsets(string id, Vector3 position)
        {
            var mainMap = ActivateMap(id, position);
            
            if (mainMap.Model.OffsetMaps != null)
            {
                foreach(var offsetModel in mainMap.Model.OffsetMaps)
                {
                    ActivateMap(offsetModel.MapFile, position + offsetModel.Offset.GetVector3());
                }
            }
        }

        private Map ActivateMap(string id, Vector3 position)
        {
            Map existingMap;
            if (_allMaps.TryGetValue(id, out existingMap))
            {
                existingMap.Activate();
                return existingMap;
            }
            else
            {
                var map = new Map(this, id);
                map.Load(position);
                _allMaps.Add(id, map);
                return map;
            }
        }

        public void LoadMap(string id, double x, double y, double z)
        {
            foreach(var map in _allMaps) map.Value.Deactivate();
            ActivateMapsWithOffsets(id, new Vector3((float)x, (float)y, (float)z));
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
