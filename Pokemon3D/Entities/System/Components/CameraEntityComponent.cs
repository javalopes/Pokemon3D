﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering;
using static GameProvider;

namespace Pokemon3D.Entities.System.Components
{
    [JsonComponentId("camera")]
    internal class CameraEntityComponent : EntityComponent
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

        public CameraEntityComponent(Entity referringEntity, Skybox skybox, int cameraMask = 1) : base(referringEntity)
        {
            Camera = GameInstance.GetService<SceneRenderer>().CreateCamera(cameraMask);
            NearClipDistance = 0.1f;
            FarClipDistance = 1000.0f;
            FieldOfView = MathHelper.PiOver4;
            ClearColor = Color.CornflowerBlue;
            Camera.Skybox = skybox;
            Camera.IsActive = ReferringEntity.IsActive;
        }

        public override void OnIsActiveChanged()
        {
            Camera.IsActive = IsActive;
        }

        public override EntityComponent Clone(Entity target)
        {
            throw new global::System.NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Camera.GlobalEulerAngles = ReferringEntity.GlobalEulerAngles;
            Camera.GlobalPosition = ReferringEntity.GlobalPosition;
            Camera.Update();
        }
    }
}
