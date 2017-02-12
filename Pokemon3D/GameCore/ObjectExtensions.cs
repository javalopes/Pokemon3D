using System;

namespace Pokemon3D.GameCore
{
    public static class ObjectExtensions
    {
        public static void QueueGameEvent(this object sender, string category, TimeSpan? delay = null)
        {
            GameProvider.IGameInstance.GetService<EventAggregator>().QueueGameEvent(new GameEvent(sender, category, delay.GetValueOrDefault(TimeSpan.Zero)));
        }

        public static void QueueGameEvent(this object sender, string category, string property1, object value1, TimeSpan? delay = null)
        {
            var gameEvent = new GameEvent(sender, category, delay.GetValueOrDefault(TimeSpan.Zero))
                                        .WithProperty(property1, value1);
            GameProvider.IGameInstance.GetService<EventAggregator>().QueueGameEvent(gameEvent);
        }
    }
}