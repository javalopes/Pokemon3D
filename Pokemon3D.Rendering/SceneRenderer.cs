using Microsoft.Xna.Framework;
using Pokemon3D.Rendering.Compositor;

namespace Pokemon3D.Rendering
{
    public interface SceneRenderer
    {
        bool EnablePostProcessing { get; set; }

        void AddPostProcessingStep(PostProcessingStep step);

        void Draw();

        RenderSettings RenderSettings { get; }

        DrawableElement CreateDrawableElement(bool initializing);

        Light CreateDirectionalLight(Vector3 direction);

        void RemoveDrawableElement(DrawableElement element);

        void OnViewSizeChanged(Rectangle oldSize, Rectangle newSize);

        Camera CreateCamera();

        /// <summary>
        /// Ambient Light for all Objects. Default is white.
        /// </summary>
        Vector4 AmbientLight { get; set; }

        void LateDebugDraw3D();
    }
}
