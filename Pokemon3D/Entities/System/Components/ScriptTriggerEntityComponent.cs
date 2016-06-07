using Microsoft.Xna.Framework;
using Pokemon3D.Collisions;
using Pokemon3D.Screens.Overworld;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Entities.System.Components
{
    [JsonComponentId("scripttrigger")]
    class ScriptTriggerEntityComponent : EntityComponent
    {
        private string _script;
        private string _trigger;
        private readonly string _message;

        private readonly InteractionPromptOverworldUIElement _uiElement;
        private bool _addedUIElement = false;

        private readonly Collider _collider;

        public ScriptTriggerEntityComponent(EntityComponentDataCreationStruct parameters)
            : base(parameters)
        {
            _script = GetDataOrDefault("Script", "");
            _trigger = GetDataOrDefault("Trigger", "Interaction");
            _message = GetDataOrDefault("Message", "Interact");

            _collider = new Collider(Parent.Scale, isTrigger: true);
            _collider.SetPosition(Parent.GlobalPosition);
            GameInstance.CollisionManager.Add(_collider);
            _collider.OnTriggerEnter = OnTriggerEnter;
            _collider.OnTriggerLeave = OnTriggerLeave;

            _uiElement = new InteractionPromptOverworldUIElement(Parent, _message);
            _uiElement.InteractionStarted += InteractionHandler;
        }

        private void OnTriggerLeave(Collider collider)
        {
            if (collider.Tag != "Player") return;
            _uiElement.IsActive = false;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.Tag != "Player") return;

            var overworldScreen = (OverworldScreen)GameInstance.ScreenManager.CurrentScreen;

            overworldScreen.AddUiElement(_uiElement);
            _uiElement.IsActive = true;
            _uiElement.Message = _message;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _collider.SetPosition(Parent.Position);
        }

        public override void OnComponentRemove()
        {
            if (_addedUIElement)
            {
                var screen = GameInstance.ScreenManager.CurrentScreen;
                if (screen is OverworldScreen) ((OverworldScreen)screen).RemoveUiElement(_uiElement);
            }
        }

        private void InteractionHandler(InteractionPromptOverworldUIElement uiElement)
        {
            ScriptPipeline.ScriptPipelineManager.RunScript(_script);
        }

        protected override void OnDataChanged(string key, string oldData, string newData)
        {
            switch (key)
            {
                case "Script":
                    _script = newData;
                    break;
            }
        }
    }
}

