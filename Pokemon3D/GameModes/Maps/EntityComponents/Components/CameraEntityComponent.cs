using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering;

namespace Pokemon3D.GameModes.Maps.EntityComponents.Components
{
    class CameraEntityComponent : EntityComponent
    {
        private Camera _camera;

        public float NearClipDistance
        {
            get { return _camera.NearClipDistance; }
            set { _camera.NearClipDistance = value; }
        }
        public float FarClipDistance
        {
            get { return _camera.FarClipDistance; }
            set { _camera.FarClipDistance = value; }
        }
        public float FieldOfView
        {
            get { return _camera.FieldOfView; }
            set { _camera.FieldOfView = value; }
        }

        public Matrix ViewMatrix { get; private set; }

        public Matrix ProjectionMatrix { get; private set; }

        public Color? ClearColor
        {
            get { return _camera.ClearColor; }
            set { _camera.ClearColor = value; }
        }
        
        public CameraEntityComponent(Entity parent, Skybox skybox) : base(parent)
        {
            _camera = Parent.Game.Renderer.CreateCamera();
            NearClipDistance = 0.1f;
            FarClipDistance = 1000.0f;
            FieldOfView = MathHelper.PiOver4;
            ClearColor = Color.CornflowerBlue;
            _camera.Skybox = skybox;
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            _camera.GlobalEulerAngles = Parent.GlobalEulerAngles;
            _camera.GlobalPosition = Parent.GlobalPosition;
        }
    }
}
