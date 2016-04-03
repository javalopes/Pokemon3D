namespace Pokemon3D.GameModes.Maps.EntityComponents.Components
{
    /// <summary>
    /// An entity component that flips through the entity's textures at a set interval.
    /// </summary>
    class AnimateTexturesEntityComponent : EntityComponent
    {
        private ModelEntityComponent _modelComponent;

        float _animationDelay;
        int _textureIndex;

        public AnimateTexturesEntityComponent(EntityComponentDataCreationStruct parameters) : base( parameters)
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
            _animationDelay -= elapsedTime;
            if (_animationDelay <= 0f)
            {
                //todo: repair.
                // Flip to next texture:
                //if (++_textureIndex >= _modelComponent.TextureSources.Length) _textureIndex = 0;

                //if (_modelComponent == null) _modelComponent = Parent.GetComponent<ModelEntityComponent>(IDs.VisualModel);
                //_modelComponent?.SetTexture(_textureIndex);

                // Reset delay after flip:
                SetInitialAnimationDelay();
            }
        }
    }
}
