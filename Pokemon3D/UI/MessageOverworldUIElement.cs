using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Common.Localization;
using Pokemon3D.Content;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Rendering.UI.Controls;
using static Pokemon3D.GameProvider;

namespace Pokemon3D.UI
{
    class MessageOverworldUiElement : UiOverlay
    {

        public MessageOverworldUiElement(string message)
        {
            var box = AddElement(new ColoredRectangle(Color.Black.Alpha(200), new Rectangle(0,0, 400, 100)));
            box.SetPosition(new Vector2(GameInstance.ScreenBounds.Width / 2 - 200, 100));

            var text =
                AddElement(new StaticText(GameInstance.GetService<ContentManager>().Load<SpriteFont>(ResourceNames.Fonts.LargeUIRegular),
                    LocalizedValue.Static(message)));
            text.SetPosition(new Vector2(GameInstance.ScreenBounds.Width / 2 - 190, 110));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (GameInstance.GetService<InputSystem.InputSystem>().IsPressedOnce(ActionNames.MenuAccept))
            {
                Hide();
            }
        }
    }
}
