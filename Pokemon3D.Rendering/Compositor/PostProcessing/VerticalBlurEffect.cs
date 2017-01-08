using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;

namespace Pokemon3D.Rendering.Compositor.PostProcessing
{
    public class VerticalBlurEffect : PostProcessEffectBase
    {
        internal VerticalBlurEffect(GameContext context, Effect postProcessEffect) : base(context, postProcessEffect, "VerticalBlur")
        {
        }
    }
}