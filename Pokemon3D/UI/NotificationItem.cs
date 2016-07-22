
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Animations;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Rendering.UI.Animations;
using static Pokemon3D.GameCore.GameProvider;
using Pokemon3D.Common.Shapes;

namespace Pokemon3D.UI
{
    class NotificationItem : UiElement
    {
        private const int ElementPadding = 5;
        private const int ElementMargin = 2;
        private const int IconSize = 16;

        private readonly SpriteFont _spriteFont;
        private readonly Color _backgroundColor;
        private readonly Texture2D _notificationIcons;
        private float _remainingLifeTime;

        private readonly Dictionary<NotificationKind, Rectangle> _notificationRectangle = new Dictionary<NotificationKind, Rectangle>
        {
            { NotificationKind.Error, new Rectangle(0,0,IconSize, IconSize) },
            { NotificationKind.Information, new Rectangle(0,IconSize,IconSize, IconSize) },
            { NotificationKind.Warning, new Rectangle(IconSize,0,IconSize, IconSize) }
        };
        
        public NotificationItem(float lifeTime, NotificationKind notificationKind, string message)
        {
            _spriteFont = GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.NotificationFont);
            _backgroundColor = new Color(70, 70, 70);
            _notificationIcons = GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.NotificationIcons);
            NotificationKind = notificationKind;
            Message = message;
            
            var transit = lifeTime*0.125f;
            _remainingLifeTime = lifeTime - 2*transit;

            EnterAnimation = new UiAlphaAnimation(transit, 0,1);
            LeaveAnimation = new UiAlphaAnimation(transit, 1, 0);
        }

        public NotificationKind NotificationKind { get; }
        public string Message { get; }
        public int Index { get; set; }
        public int Width { get; set; }

        public override bool IsInteractable => false;

        public override void Update(GameTime time)
        {
            base.Update(time);
            if (State != UiState.Active) return;

            _remainingLifeTime -= time.GetSeconds();
            if (_remainingLifeTime <= 0.0f) Hide();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var elementHeight = _spriteFont.LineSpacing + 2 * ElementPadding;

            var startY = GameInstance.ScreenBounds.Height - (Index+1) * (elementHeight + ElementMargin);
            var startX = (GameInstance.ScreenBounds.Width - Width) / 2;

            GameInstance.GetService<ShapeRenderer>().DrawRectangle(startX, startY, Width, elementHeight, _backgroundColor * Alpha);

            var currentX = startX + ElementMargin;
            var sourceRectangle = _notificationRectangle[NotificationKind];
            var position = new Vector2(currentX, startY + (elementHeight - IconSize) / 2);
            spriteBatch.Draw(_notificationIcons, position, sourceRectangle, Color.White * Alpha);

            position = new Vector2(currentX + IconSize + ElementMargin, startY + ElementPadding);
            spriteBatch.DrawString(_spriteFont, Message, position, Color.White * Alpha);
        }
    }
}
