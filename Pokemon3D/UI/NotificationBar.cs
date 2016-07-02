using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Pokemon3D.Rendering.UI;

namespace Pokemon3D.UI
{
    class NotificationBar : UiCompoundElement
    {
        private readonly int _maxNotifications;
        private readonly float _notificationTime;
        private readonly List<NotificationItem> _notifications = new List<NotificationItem>();
        private readonly int _barWidth;

        public NotificationBar(int barWidth, int maxNotifications = 10)
        {
            _barWidth = barWidth;
            _maxNotifications = maxNotifications;
            _notificationTime = 2.0f;
        }

        public void PushNotification(NotificationKind notificationKind, string message)
        {
            var notificationItem = new NotificationItem(_notificationTime, notificationKind, message)
            {
                Width = _barWidth
            };
            notificationItem.Show();
            _notifications.Add(notificationItem);
            AddChildElement(notificationItem);
            if (_notifications.Count > _maxNotifications)
            {
                RemoveChild(_notifications.First());
                _notifications.RemoveAt(0);
            }
            UpdateIndices();
        }

        private void UpdateIndices()
        {
            var index = 0;
            foreach (var notificationItem in _notifications)
            {
                notificationItem.Index = index++;
            }
        }

        public override void Update(GameTime gameTime)
        {
            _notifications.ForEach(n => n.Update(gameTime));

            var elementsToRemove = _notifications.Where(n => n.State == UiState.Inactive).ToArray();
            if (elementsToRemove.Length > 0)
            {
                foreach (var element in elementsToRemove)
                {
                    _notifications.Remove(element);
                    RemoveChild(element);
                }
                UpdateIndices();
            }
            
        }

        public override bool IsInteractable => false;
    }
}
