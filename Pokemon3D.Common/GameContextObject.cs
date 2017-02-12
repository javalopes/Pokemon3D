namespace Pokemon3D.Common
{
    public abstract class GameContextObject
    {
        public IGameContext GameContext { get; private set; }

        protected GameContextObject(IGameContext iGameContext)
        {
            GameContext = iGameContext;
        }
    }
}
