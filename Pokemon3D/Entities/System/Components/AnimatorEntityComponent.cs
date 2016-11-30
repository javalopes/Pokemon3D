using System;
using Microsoft.Xna.Framework;
using Pokemon3D.Common.Animations;

namespace Pokemon3D.Entities.System.Components
{
    internal class AnimatorEntityComponent : EntityComponent
    {
        private readonly Animator _animator = new Animator();

        public AnimatorEntityComponent(Entity referringEntity) : base(referringEntity)
        {

        }

        public AnimatorEntityComponent(EntityComponentDataCreationStruct parameter) : base(parameter)
        {

        }

        public void AddAnimation(string animationName, Animation animation)
        {
            _animator.AddAnimation(animationName, animation);
        }

        public void PlayAnimation(string animationName)
        {
            _animator.SetAnimation(animationName);
        }

        public void StopAnimation()
        {
            _animator.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            _animator.Update(gameTime);
            base.Update(gameTime);
        }
        public override EntityComponent Clone(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}
