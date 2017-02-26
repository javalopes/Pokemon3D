namespace Pokemon3D.Common
{
    public interface IGameContext
    {
        TService GetService<TService>() where TService : class;
    }
}
