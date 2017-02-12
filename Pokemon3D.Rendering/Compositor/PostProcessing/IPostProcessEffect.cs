using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Compositor.PostProcessing
{
    public interface IPostProcessEffect
    {
        RenderTarget2D Process(SpriteBatch spriteBatch, Vector2 invScreenSize, RenderTarget2D source);
    }
}