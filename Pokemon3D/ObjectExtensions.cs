using System;
using Pokemon3D.GameCore;

namespace Pokemon3D
{
    public static class ObjectExtensions
    {
        public static void QueueGameEvent(this object sender, string category, TimeSpan? delay = null)
        {
            GameProvider.GameInstance.GetService<EventAggregator>().QueueGameEvent(new GameEvent(sender, category, delay.GetValueOrDefault(TimeSpan.Zero)));
        }

        public static void QueueGameEvent(this object sender, string category, string property1, object value1, TimeSpan? delay = null)
        {
            var gameEvent = new GameEvent(sender, category, delay.GetValueOrDefault(TimeSpan.Zero))
                                        .WithProperty(property1, value1);
            GameProvider.GameInstance.GetService<EventAggregator>().QueueGameEvent(gameEvent);
        }
    }
}