using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;

namespace Pokemon3D.Rendering.Compositor.PostProcessing
{
    public class HorizontalBlurEffect : PostProcessEffectBase
    {
        internal HorizontalBlurEffect(GameContext context, Effect postProcessEffect) : base(context, postProcessEffect, "HorizontalBlur")
        {
        }
    }
}