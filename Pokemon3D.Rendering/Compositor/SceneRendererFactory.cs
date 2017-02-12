using Pokemon3D.Common;

namespace Pokemon3D.Rendering.Compositor
{
    public static class SceneRendererFactory
    {
        public static SceneRenderer Create(IGameContext iGameContext, EffectProcessor effectProcessor, RenderSettings settings)
        {
            return new ForwardSceneRenderer(iGameContext, effectProcessor, settings);
        }
    }
}
