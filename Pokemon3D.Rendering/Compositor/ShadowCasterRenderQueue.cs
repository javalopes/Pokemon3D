using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering.Compositor
{
    internal class ShadowCasterRenderQueue : RenderQueue
    {
        public ShadowCasterRenderQueue(IGameContext context, EffectProcessor effectProcessor) : base(context, effectProcessor)
        {
        }

        protected override EffectPassCollection GetPasses(Material material, RenderSettings renderSettings)
        {
            return EffectProcessor.GetShadowDepthPass(material);
        }
    }
}