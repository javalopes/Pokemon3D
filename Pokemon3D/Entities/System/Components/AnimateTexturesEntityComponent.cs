using Pokemon3D.Common.Animations;
using System.Linq;
using System;

namespace Pokemon3D.Entities.System.Components
{
    /// <summary>
    /// An entity component that flips through the entity's textures at a set interval.
    /// </summary>
    [JsonComponentId("animatedtextures")]
    class AnimateTexturesEntityComponent : AnimatorEntityComponent
    {
        private ModelEntityComponent _modelComponent = null;
        private int _textureIndex;

        public AnimateTexturesEntityComponent(EntityComponentDataCreationStruct parameters) : base(parameters)
        {
            _textureIndex = 0;

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
