namespace Pokemon3D.Common
{
    public abstract class GameContextObject
    {
        public IGameContext IGameContext { get; private set; }

        protected GameContextObject(IGameContext iGameContext)
        {
            IGameContext = iGameContext;
        }
    }
}
