using System;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.DataModel.GameMode.Map;
using Pokemon3D.UI;
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
        public EntitySystem EntitySystem { get; }

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
            ActiveMap = new Map(this, mapModel, _onFinished);
            Player = new Player(this);
        }

        public void ActivateNewEntities()
        {
            EntitySystem.InitializeAllPendingEntities();
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
