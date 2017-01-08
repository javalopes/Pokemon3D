﻿using Microsoft.Xna.Framework;

namespace Pokemon3D.Rendering
{
    public interface SceneRenderer
    {
        void Draw();

        RenderSettings RenderSettings { get; }

        DrawableElement CreateDrawableElement(bool initializing, int cameraMask = 1);

        Light CreateDirectionalLight(Vector3 direction);

        void RemoveDrawableElement(DrawableElement element);

        void OnViewSizeChanged(Rectangle oldSize, Rectangle newSize);

        Camera CreateCamera(int cameraMask = 1);

        void RemoveCamera(Camera camera);

        /// <summary>
        /// Ambient Light for all Objects. Default is white.
        /// </summary>
        Vector4 AmbientLight { get; set; }

        void LateDebugDraw3D();

        void RemoveLight(Light mainLight);
    }
}
