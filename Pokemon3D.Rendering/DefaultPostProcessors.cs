using Pokemon3D.Common;
using Pokemon3D.Rendering.Compositor;
using Pokemon3D.Rendering.Compositor.PostProcessing;

namespace Pokemon3D.Rendering
{
    public class DefaultPostProcessors
    {
        public HorizontalBlurEffect HorizontalBlur { get; }
        public VerticalBlurEffect VerticalBlur { get; }

        internal DefaultPostProcessors(GameContext context, EffectProcessor processor)
        {
            HorizontalBlur = new HorizontalBlurEffect(context, processor.PostProcessingEffect);
            VerticalBlur = new VerticalBlurEffect(context, processor.PostProcessingEffect);
        }
    }
}