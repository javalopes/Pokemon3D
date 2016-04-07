namespace Pokemon3D.Entities.System.Components
{
    /// <summary>
    /// An entity component that flips through the entity's textures at a set interval.
    /// </summary>
    [JsonComponentId("animatedtextures")]
    class AnimateTexturesEntityComponent : EntityComponent
    {
        private ModelEntityComponent _modelComponent = null;

        float _animationDelay;
        int _textureIndex;

        public AnimateTexturesEntityComponent(EntityComponentDataCreationStruct parameters) : base(parameters)
        {
            SetInitialAnimationDelay();
            _textureIndex = 0;
        }

        private void SetInitialAnimationDelay()
        {
            _animationDelay = GetData<float>("AnimationDelay");
        }

        public override void Update(float elapsedTime)
        {
            _modelComponent = Parent.GetComponent<ModelEntityComponent>();

            if (_modelComponent != null)
            {
                _animationDelay -= elapsedTime;
                if (_animationDelay <= 0f)
                {
                    _textureIndex++;
                    if (_textureIndex >= _modelComponent.Regions.Count)
                        _textureIndex = 0;

                    _modelComponent.SetTexture(_textureIndex);

                    // Reset delay after flip:
                    SetInitialAnimationDelay();
                }
            }
            else
            {
                // when no model entity component is found, reset the animation parameters.
                SetInitialAnimationDelay();
                _textureIndex = 0;
            }
        }
    }
}
