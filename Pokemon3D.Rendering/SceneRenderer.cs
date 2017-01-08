using Microsoft.Xna.Framework;

namespace Pokemon3D.Rendering
{
    public interface SceneRenderer
    {
        RenderSettings RenderSettings { get; }

        Vector4 AmbientLight { get; set; }

        DefaultPostProcessors DefaultPostProcessors { get; }

        DrawableElement CreateDrawableElement(bool initializing, int cameraMask = 1);

        Light CreateDirectionalLight(Vector3 direction);

        Camera CreateCamera(int cameraMask = 1);

        void RemoveDrawableElement(DrawableElement element);

        void RemoveCamera(Camera camera);

        void RemoveLight(Light mainLight);

        void OnViewSizeChanged(Rectangle oldSize, Rectangle newSize);

        void LateDebugDraw3D();

        void Draw();
    }
}
