using Pokemon3D.GameCore;

namespace Pokemon3D
{
    public static class ObjectExtensions
    {
        public static void SendGameEvent(this object sender, string name)
        {
            GameProvider.GameInstance.SendEvent(new GameEvent(sender, name));
        }
    }
}