using Pokemon3D.Common;

namespace Pokemon3D.Rendering.Compositor
{
    public static class SceneRendererFactory
    {
        public static SceneRenderer Create(GameContext gameContext, EffectProcessor effectProcessor, RenderSettings settings)
        {
            return new ForwardSceneRenderer(gameContext, effectProcessor, settings);
        }
    }
}
