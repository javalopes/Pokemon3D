using Pokemon3D.Common.Animations;
using System.Linq;

namespace Pokemon3D.Entities.System.Components
{
    /// <summary>
    /// An entity component that flips through the entity's textures at a set interval.
    /// </summary>
    [JsonComponentId("animatedtextures")]
    internal class AnimateTexturesEntityComponent : AnimatorEntityComponent
    {
        private ModelEntityComponent _modelComponent = null;

        public AnimateTexturesEntityComponent(EntityComponentDataCreationStruct parameters) : base(parameters)
        {
            var animationDuration = GetData<float>("AnimationDuration");

            var frameCount = GetData<int>("FrameCount");
            var looping = GetDataOrDefault("Loop", true);

            AddAnimation("Default", Animation.CreateDiscrete(animationDuration, Enumerable.Range(0, frameCount).ToArray(), OnUpdateAnimationFrame, looping));
            PlayAnimation("Default");
        }

        private void OnUpdateAnimationFrame(int textureIndex)
        {
            _modelComponent = _modelComponent ?? Parent.GetComponent<ModelEntityComponent>();
            if (_modelComponent != null && textureIndex < _modelComponent.Regions.Count)
            {
                _modelComponent.SetTexture(textureIndex);
            }
        }
    }
}
