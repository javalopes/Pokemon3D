using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Pokemon3D.GameCore
{
    class EventAggregator
    {
        public event Action<GameEvent> GameEventRaised;
        
        private readonly object _lockObject = new object();
        private readonly List<GameEvent> _gameEvents = new List<GameEvent>();

        public void QueueGameEvent(GameEvent gameEvent)
        {
            lock (_lockObject)
            {
                _gameEvents.Add(gameEvent);
            }
        }

        public void Update(GameTime gameTime)
        {
            lock (_lockObject)
            {
                foreach (var gameEvent in _gameEvents) gameEvent.Delay -= gameTime.ElapsedGameTime;
                foreach (var gameEvent in _gameEvents.Where(g => g.Delay <= TimeSpan.Zero)) GameEventRaised?.Invoke(gameEvent);
                _gameEvents.RemoveAll(g => g.Delay <= TimeSpan.Zero);
            }
        }
    }
}
