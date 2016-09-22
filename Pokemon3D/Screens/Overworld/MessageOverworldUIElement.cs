using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Common.Input;
using Pokemon3D.Common.Shapes;
using Pokemon3D.Content;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Screens.Overworld
{
    class MessageOverworldUIElement : OverworldUIElement
    {
        private readonly string _message;

        public override bool IsBlocking => true;

        public event Action<MessageOverworldUIElement> MessageClosed;

        public MessageOverworldUIElement(string message)
        {
            _message = message;
        }

        public override void Draw(GameTime gameTime)
        {
            GameInstance.GetService<ShapeRenderer>().DrawRectangle(new Rectangle(GameInstance.ScreenBounds.Width / 2 - 200, 100, 400, 100), Color.Black.Alpha(200));

            var font = GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.LargeUIRegular);
            var fontSize = font.MeasureString(_message);

            GameInstance.GetService<SpriteBatch>().DrawString(font, _message, new Vector2(GameInstance.ScreenBounds.Width / 2 - 190, 110), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (GameInstance.GetService<InputSystem>().GamePad.IsButtonDownOnce(Microsoft.Xna.Framework.Input.Buttons.A))
            {
                IsActive = false;
                MessageClosed?.Invoke(this);
            }
        }
    }
}
