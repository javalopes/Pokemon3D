using System;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.DataModel.GameMode.Map;
using Pokemon3D.UI;
using System.Collections.Generic;
using Pokemon3D.Entities.System;
using Microsoft.Xna.Framework;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Entities
{
    class World
    {
        private Action _onFinished;

        public Map ActiveMap { get; private set; }
        public Player Player { get; private set; }

        public EntitySystem EntitySystem { get; private set; }

        private List<Entity> _entitiesToActivate = new List<Entity>();

        public World()
        {
            EntitySystem = new EntitySystem();
        }

        public void StartNewGameAsync(Action onFinished)
        {
            _onFinished = onFinished;
            GameInstance.ActiveGameMode.PreloadAsync(ContinueLoadMap);
        }

        private void ContinueLoadMap()
        {
            GameInstance.ActiveGameMode.LoadMapAsync(GameInstance.ActiveGameMode.GameModeInfo.StartMap, FinishedLoadingMapModel);
        }

        private void FinishedLoadingMapModel(MapModel mapModel)
        {
            ActiveMap = new Map(this, mapModel);
            Player = new Player(this);
            _onFinished();
        }

        public void AddEntityToActivate(Entity entity)
        {
            _entitiesToActivate.Add(entity);
        }

        public void ActivateNewEntities()
        {
            foreach (var entity in _entitiesToActivate)
            {
                entity.IsActive = true;
            }
            _entitiesToActivate.Clear();
        }

        public void Update(GameTime gameTime)
        {
            EntitySystem.Update(gameTime);
            Player.Update(gameTime);

            if (GameInstance.InputSystem.Keyboard.IsKeyDownOnce(Keys.V))
            {
                if (Player.MovementMode == PlayerMovementMode.GodMode)
                {
                    GameInstance.NotificationBar.PushNotification(NotificationKind.Information, "Disabled God Mode");
                }
                Player.MovementMode = Player.MovementMode == PlayerMovementMode.FirstPerson ? PlayerMovementMode.ThirdPerson : PlayerMovementMode.FirstPerson;
            }

            if (GameInstance.InputSystem.Keyboard.IsKeyDownOnce(Keys.F10))
            {
                Player.MovementMode = PlayerMovementMode.GodMode;
                GameInstance.NotificationBar.PushNotification(NotificationKind.Information, "Enabled God Mode");
            }
        }
    }
}
