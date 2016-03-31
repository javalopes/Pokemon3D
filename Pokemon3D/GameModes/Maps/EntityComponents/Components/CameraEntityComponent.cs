using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering;

namespace Pokemon3D.GameModes.Maps.EntityComponents.Components
{
    class CameraEntityComponent : EntityComponent
    {
        public float NearClipDistance
        {
            get { return Camera.NearClipDistance; }
            set { Camera.NearClipDistance = value; }
        }
        public float FarClipDistance
        {
            get { return Camera.FarClipDistance; }
            set { Camera.FarClipDistance = value; }
        }
        public float FieldOfView
        {
            get { return Camera.FieldOfView; }
            set { Camera.FieldOfView = value; }
        }

        public Matrix ViewMatrix { get; private set; }

        public Matrix ProjectionMatrix { get; private set; }

        public Color? ClearColor
        {
            get { return Camera.ClearColor; }
            set { Camera.ClearColor = value; }
        }

        public Camera Camera { get; }

        public CameraEntityComponent(Entity parent, Skybox skybox) : base(parent)
        {
            Camera = Parent.Game.Renderer.CreateCamera();
            NearClipDistance = 0.1f;
            FarClipDistance = 1000.0f;
            FieldOfView = MathHelper.PiOver4;
            ClearColor = Color.CornflowerBlue;
            Camera.Skybox = skybox;
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            Camera.GlobalEulerAngles = Parent.GlobalEulerAngles;
            Camera.GlobalPosition = Parent.GlobalPosition;
            Camera.Update();
        }
    }
}
