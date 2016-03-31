using System;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.DataModel.GameMode.Map;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes.Maps;
using Pokemon3D.UI;

namespace Pokemon3D.GameModes
{
    class World : GameObject
    {
        private Action _onFinished;
        
        public Map ActiveMap { get; private set; }
        public Player Player { get; private set; }

        public void StartNewGameAsync(Action onFinished)
        {
            _onFinished = onFinished;
            Game.ActiveGameMode.PreloadAsync(ContinueLoadMap);
        }

        private void ContinueLoadMap()
        {
            Game.ActiveGameMode.LoadMapAsync(Game.ActiveGameMode.GameModeInfo.StartMap, FinishedLoadingMapModel);
        }

        private void FinishedLoadingMapModel(MapModel mapModel)
        {
            ActiveMap = new Map(Game.ActiveGameMode, mapModel);
            Player = new Player(Game.EntitySystem);
            _onFinished();
        }

        public void Update(float elapsedTime)
        {
            Player.Update(elapsedTime);

            if (Game.InputSystem.Keyboard.IsKeyDownOnce(Keys.V))
            {
                if (Player.MovementMode == PlayerMovementMode.GodMode)
                {
                    Game.NotificationBar.PushNotification(NotificationKind.Information, "Disabled God Mode");
                }
                Player.MovementMode = Player.MovementMode == PlayerMovementMode.FirstPerson ? PlayerMovementMode.ThirdPerson : PlayerMovementMode.FirstPerson;
            }

            if (Game.InputSystem.Keyboard.IsKeyDownOnce(Keys.F10))
            {
                Player.MovementMode = PlayerMovementMode.GodMode;
                Game.NotificationBar.PushNotification(NotificationKind.Information, "Enabled God Mode");
            }
        }
    }
}
