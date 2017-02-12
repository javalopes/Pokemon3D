using Pokemon3D.Common;

namespace Pokemon3D.Rendering.Compositor
{
    public static class SceneRendererFactory
    {
        public static ISceneRenderer Create(IGameContext iGameContext, EffectProcessor effectProcessor, RenderSettings settings)
        {
            return new ForwardISceneRenderer(iGameContext, effectProcessor, settings);
        }
    }
}
