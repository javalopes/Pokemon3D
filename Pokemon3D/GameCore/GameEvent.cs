using System;

namespace Pokemon3D.GameCore
{
    public class GameEvent
    {
        public const string GameQuitToMainMenu = "GAME_QUIT_TO_MAIN_MENU";

        public GameEvent(object sender, string name, TimeSpan delay)
        {
            Sender = sender;
            Name = name;
            Delay = delay;
        }

        public string Name { get; private set; }

        public object Sender { get; private set; }

        public TimeSpan Delay { get; set; }
    }
}