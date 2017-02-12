using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;

namespace Pokemon3D.Rendering.Compositor.PostProcessing
{
    public class VerticalBlurEffect : IPostProcessEffectBase
    {
        internal VerticalBlurEffect(IGameContext context, Effect postProcessEffect) : base(context, postProcessEffect, "VerticalBlur")
        {
        }
    }
}