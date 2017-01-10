using Microsoft.Xna.Framework;
using Pokemon3D.Collisions;
using Pokemon3D.Screens;
using Pokemon3D.UI;
using static Pokemon3D.GameProvider;

namespace Pokemon3D.Entities.System.Components
{
    [JsonComponentId("scripttrigger")]
    internal class ScriptTriggerEntityComponent : EntityComponent
    {
        private string _script;

        private readonly InteractionPromptOverworldUiElement _uiElement;
        private bool _addedUiElement;

        private readonly Collider _collider;

        public ScriptTriggerEntityComponent(EntityComponentDataCreationStruct parameters)
            : base(parameters)
        {
            _script = GetDataOrDefault("Script", "");
            var message = GetDataOrDefault("Message", "Interact");

            _collider = new Collider(ReferringEntity.Scale, isTrigger: true);
            _collider.SetPosition(ReferringEntity.GlobalPosition);
            GameInstance.GetService<CollisionManager>().Add(_collider);
            _collider.OnTriggerEnter = OnTriggerEnter;
            _collider.OnTriggerLeave = OnTriggerLeave;

            _uiElement = new InteractionPromptOverworldUiElement(ReferringEntity.GlobalPosition, message);
            _uiElement.InteractionStarted += InteractionHandler;
        }

        private void OnTriggerLeave(Collider collider)
        {
            if (collider.Tag != "Player") return;
            _uiElement.Hide();
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.Tag != "Player") return;

            var currentScreen = GameInstance.GetService<ScreenManager>().CurrentScreen;

            if (!_addedUiElement)
            {
                currentScreen.AddOverlay(_uiElement);
                _addedUiElement = true;
            }

            _uiElement.Show();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _collider.SetPosition(ReferringEntity.Position);
        }

        public override void OnComponentRemove()
        {
            if (_addedUiElement)
            {
                var screen = GameInstance.GetService<ScreenManager>().CurrentScreen;
                if (screen is OverworldScreen) screen.RemoveOverlay(_uiElement);
                _addedUiElement = false;
            }
        }

        private void InteractionHandler(InteractionPromptOverworldUiElement uiElement)
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

        public override EntityComponent Clone(Entity target)
        {
            throw new global::System.NotImplementedException();
        }
    }
}

