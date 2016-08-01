using Microsoft.Xna.Framework;
using Pokemon3D.Common.Animations;
using System;

namespace Pokemon3D.Entities.System.Components
{
    internal class FigureMovementAnimationComponent : AnimatorEntityComponent
    {
        private ModelEntityComponent _modelEntityComponent;
        private const string ForwardAnimation = "WalkForward";
        private const string BackwardAnimation = "WalkBackward";
        private const string LeftAnimation = "WalkLeft";
        private const string RightAnimation = "WalkRight";

        public FigureMovementAnimationComponent(Entity parent, Animation forward, Animation backward, Animation left, Animation right) : base(parent)
        {
            if (forward == null) throw new ArgumentNullException(nameof(forward));

            AddAnimation(ForwardAnimation, forward);
            AddAnimation(BackwardAnimation, backward ?? forward);
            AddAnimation(LeftAnimation, left ?? forward);
            AddAnimation(RightAnimation, right ?? forward);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _modelEntityComponent = _modelEntityComponent ?? Parent.GetComponent<ModelEntityComponent>();

            var movementDirection = Parent.LastTranslation;

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
