using System;
using Pokemon3D.GameCore;

namespace Pokemon3D
{
    public static class ObjectExtensions
    {
        public static void QueueGameEvent(this object sender, string name, TimeSpan? delay = null)
        {
            GameProvider.GameInstance.QueueGameEvent(new GameEvent(sender, name, delay.GetValueOrDefault(TimeSpan.Zero)));
        }
    }
}