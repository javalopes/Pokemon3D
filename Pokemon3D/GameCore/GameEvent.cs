namespace Pokemon3D.GameCore
{
    public class GameEvent
    {
        public const string GameQuitToMainMenu = "GAME_QUIT_TO_MAIN_MENU";

        public GameEvent(object sender, string name)
        {
            Sender = sender;
            Name = name;
        }

        public string Name { get; private set; }

        public object Sender { get; private set; }
    }
}