using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Compositor;
using Pokemon3D.Rendering.Compositor.PostProcessing;

namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Specialized Scene Node representing a camera.
    /// </summary>
    public class Camera
    {
        public int CameraMask { get; }
        public float NearClipDistance { get; set; }
        public float FarClipDistance { get; set; }
        public float FieldOfView { get; set; }
        public Viewport Viewport { get; set; }
        public bool IsActive { get; set; }

        public Matrix ViewMatrix { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }
        public Vector3 GlobalEulerAngles { get; set; }
        public Vector3 GlobalPosition { get; set; }
        public BoundingFrustum Frustum { get; }

        public Color? ClearColor { get; set; }
        public float? DepthClear { get; set; }
        public Skybox Skybox { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        public bool UseCulling { get; set; }
        public PostProcess PostProcess { get; }
        public bool IsMain { get; set; }
                     
        internal Camera(Viewport viewport, int cameraMask)
        {
            DepthClear = 1;
            DepthStencilState = null;
            CameraMask = cameraMask;
            Viewport = viewport;
            NearClipDistance = 0.1f;
            FarClipDistance = 1000.0f;
            FieldOfView = MathHelper.PiOver4;
            Frustum = new BoundingFrustum(Matrix.Identity);
            ClearColor = Color.CornflowerBlue;
            IsActive = true;
            UseCulling = true;
            PostProcess = new PostProcess();
        }

        public void Update()
        {
            ViewMatrix = Matrix.Invert(Matrix.CreateFromYawPitchRoll(GlobalEulerAngles.Y, GlobalEulerAngles.X, GlobalEulerAngles.Z) * Matrix.CreateTranslation(GlobalPosition));
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(FieldOfView, Viewport.AspectRatio, NearClipDistance, FarClipDistance);
            Frustum.Matrix = ViewMatrix * ProjectionMatrix;
        }

        internal void OnViewSizeChanged(Rectangle oldSize, Rectangle newSize)
        {
            Viewport = new Viewport(0,0,newSize.Width, newSize.Height);
        }

        public Vector3 ProjectWorldToScreen(Vector3 worldPosition)
        {
            return Viewport.Project(worldPosition, ProjectionMatrix, ViewMatrix, Matrix.Identity);
        }
    }
}
