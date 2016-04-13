using Microsoft.Xna.Framework;
using Pokemon3D.Common.Animations;

namespace Pokemon3D.Entities.System.Components
{
    class AnimatorEntityComponent : EntityComponent
    {
        private readonly Animator _animator = new Animator();

        public AnimatorEntityComponent(Entity parent) : base(parent)
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
    }
}
