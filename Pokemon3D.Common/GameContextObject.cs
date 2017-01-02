namespace Pokemon3D.Common
{
    public abstract class GameContextObject
    {
        public GameContext GameContext { get; private set; }

        protected GameContextObject(GameContext gameContext)
        {
            GameContext = gameContext;
        }
    }
}
