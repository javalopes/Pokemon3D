using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;

namespace Pokemon3D.Rendering.UI.Controls
{
    public class SimpleButton : UiElement
    {
        public SimpleButton(Texture2D texture, Rectangle? sourceRectangle = null) : base(texture, sourceRectangle)
        {
            var bounds = texture.Bounds;
            Bounds = bounds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawTexture(spriteBatch);
        }
    }
}
