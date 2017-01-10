using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Animations;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Entities.System;
using Pokemon3D.Entities.System.Components;

namespace Pokemon3D.Entities.Components
{
    internal class FigureMovementAnimationComponent : AnimatorEntityComponent
    {
        private ModelEntityComponent _modelEntityComponent;
        private const string ForwardAnimation = "WalkForward";
        private const string BackwardAnimation = "WalkBackward";
        private const string LeftAnimation = "WalkLeft";
        private const string RightAnimation = "WalkRight";

        public FigureMovementAnimationComponent(Entity referringEntity, Texture2D texture) : base(referringEntity)
        {
            var forward = Animation.CreateDiscrete(0.65f, new[]
           {
                texture.GetTexcoordsFromPixelCoords(0, 0),
                texture.GetTexcoordsFromPixelCoords(32, 0),
                texture.GetTexcoordsFromPixelCoords(0, 0),
                texture.GetTexcoordsFromPixelCoords(64, 0),
            }, OnUpdate, true);
            var left = Animation.CreateDiscrete(0.65f, new[]
            {
                texture.GetTexcoordsFromPixelCoords(0, 32),
                texture.GetTexcoordsFromPixelCoords(32, 32),
                texture.GetTexcoordsFromPixelCoords(0, 32),
                texture.GetTexcoordsFromPixelCoords(64, 32),
            }, OnUpdate, true);
            var right = Animation.CreateDiscrete(0.65f, new[]
            {
                texture.GetTexcoordsFromPixelCoords(0, 96),
                texture.GetTexcoordsFromPixelCoords(32, 96),
                texture.GetTexcoordsFromPixelCoords(0, 96),
                texture.GetTexcoordsFromPixelCoords(64, 96),
            }, OnUpdate, true);
            var backward = Animation.CreateDiscrete(0.65f, new[]
            {
                texture.GetTexcoordsFromPixelCoords(0, 64),
                texture.GetTexcoordsFromPixelCoords(32, 64),
                texture.GetTexcoordsFromPixelCoords(0, 64),
                texture.GetTexcoordsFromPixelCoords(64, 64),
            }, OnUpdate, true);

            AddAnimation(ForwardAnimation, forward);
            AddAnimation(BackwardAnimation, backward ?? forward);
            AddAnimation(LeftAnimation, left ?? forward);
            AddAnimation(RightAnimation, right ?? forward);
        }

        private void OnUpdate(Vector2 offset)
        {
            ReferringEntity.GetComponent<ModelEntityComponent>().Material.TexcoordOffset = offset;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _modelEntityComponent = _modelEntityComponent ?? ReferringEntity.GetComponent<ModelEntityComponent>();

            var movementDirection = ReferringEntity.LastTranslation;

            if (movementDirection.LengthSquared() > 0.0f)
            {
                if (movementDirection.X > 0.0f)
                {
                    PlayAnimation(RightAnimation);
                }
                else if (movementDirection.X < 0.0f)
                {
                    PlayAnimation(LeftAnimation);
                }
                else
                {
                    PlayAnimation(movementDirection.Z > 0.0f ? ForwardAnimation : BackwardAnimation);
                }
            }
            else
            {
                StopAnimation();
                if (_modelEntityComponent != null) _modelEntityComponent.Material.TexcoordOffset = Vector2.Zero;
            }
        }
    }
}
