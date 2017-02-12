using System;
using Microsoft.Xna.Framework;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering
{
    public interface ISceneRenderer
    {
        RenderSettings RenderSettings { get; }

        Vector4 AmbientLight { get; set; }

        DefaultPostProcessors DefaultPostProcessors { get; }

        DrawableElement CreateDrawableElement(bool initializing, int cameraMask = 1);

        Light CreateDirectionalLight(Vector3 direction);

        Camera CreateCamera(int cameraMask = 1);

        Camera GetMainCamera();

        void RemoveDrawableElement(DrawableElement element);

        void RemoveCamera(Camera camera);

        void RemoveLight(Light mainLight);

        void OnViewSizeChanged(Rectangle oldSize, Rectangle newSize);

        void Draw();

        void RegisterCustomDraw(Action<Camera, ISceneRenderer> onDraw);

        void DrawImmediate(Camera camera, Matrix world, Material material, Mesh mesh);
    }
}
