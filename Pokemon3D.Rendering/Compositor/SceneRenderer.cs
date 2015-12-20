using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Compositor
{
    public interface SceneRenderer
    {
        Vector3 LightDirection { get; set; }

        Vector4 AmbientLight { get; set; }

        bool EnablePostProcessing { get; set; }

        void AddPostProcessingStep(PostProcessingStep step);

        void Draw(Scene scene);

         RenderSettings RenderSettings { get; }
    }
}
