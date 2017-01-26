using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pokemon3D.GameCore
{
    class EventAggregator
    {
        private readonly object _lockObject = new object();

        private readonly Dictionary<Type, List<Delegate>> _delegatesByType = new Dictionary<Type, List<Delegate>>();
        private readonly List<Delegate> _currentDispatchList = new List<Delegate>();

        private readonly List<GameEvent> _gameEvents = new List<GameEvent>();
        
        public void QueueGameEvent(GameEvent gameEvent)
        {
            lock (_lockObject)
            {
                _gameEvents.Add(gameEvent);
            }
        }

        public void Subscribe<TObjectType>(Action<TObjectType> eventHandler) where TObjectType : GameEvent
        {
            List<Delegate> list;

            var eventType = typeof(TObjectType);

            if (!_delegatesByType.TryGetValue(eventType, out list))
            {
                list = new List<Delegate>();
                _delegatesByType[eventType] = list;
            }

            list.Add(eventHandler);
        }
        
        public void Unsubscribe<T>(Action<T> eventHandler) where T : GameEvent
        {
            List<Delegate> list;
            if (_delegatesByType.TryGetValue(typeof(T), out list))
            {
                list.Remove(eventHandler);
            }
        }

        public void Update(GameTime gameTime)
        {
            lock (_lockObject)
            {
                foreach (var gameEvent in _gameEvents)
                {
                    gameEvent.Delay -= gameTime.ElapsedGameTime;
                }

                foreach (var gameEvent in _gameEvents)
                {
                    if (gameEvent.Delay > TimeSpan.Zero) continue;

                    List<Delegate> subscribers;
                    if (!_delegatesByType.TryGetValue(gameEvent.GetType(), out subscribers)) continue;

                    _currentDispatchList.Clear();
                    _currentDispatchList.AddRange(subscribers);
                    foreach (var delegateToInvoke in _currentDispatchList)
                    {
                        delegateToInvoke.DynamicInvoke(gameEvent);
                    }
                }
                _gameEvents.RemoveAll(g => g.Delay <= TimeSpan.Zero);
            }
        }
    }
}
