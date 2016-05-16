using Microsoft.Xna.Framework;
using Pokemon3D.Collisions;
using Pokemon3D.Screens.Overworld;
using System.Threading;
using static Pokemon3D.GameCore.GameProvider;
using System.IO;
using Pokemon3D.Common.Diagnostics;

namespace Pokemon3D.Entities.System.Components
{
    [JsonComponentId("scripttrigger")]
    class ScriptTriggerEntityComponent : EntityComponent
    {
        private string _script;
        private string _trigger;
        private string _message;

        private readonly InteractionPromptOverworldUIElement _uiElement;
        private bool _addedUIElement = false;

        private readonly Collider _collider;

        public ScriptTriggerEntityComponent(EntityComponentDataCreationStruct parameters)
            : base(parameters)
        {
            _script = GetDataOrDefault("Script", "");
            _trigger = GetDataOrDefault("Trigger", "Interaction");
            _message = GetDataOrDefault("Message", "Interact");

            _collider = new Collider(Parent.Scale);
            GameInstance.CollisionManager.Add(_collider);

            _uiElement = new InteractionPromptOverworldUIElement(Parent, _message);
            _uiElement.InteractionStarted += InteractionHandler;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var screen = GameInstance.ScreenManager.CurrentScreen;
            if (screen is OverworldScreen)
            {
                var overworldScreen = (OverworldScreen)screen;
                var player = overworldScreen.ActiveWorld.Player;
                var camera = player.Camera;

                _collider.SetPosition(Parent.Position);
                var collisionResult =  _collider.CheckCollision(player.Collider);
                if (collisionResult.Collides)
                {
                    if (!_addedUIElement)
                    {
                        overworldScreen.AddUIElement(_uiElement);
                        _addedUIElement = true;
                    }
                    _uiElement.IsActive = true;
                    _uiElement.Message = _message;
                }
                else
                {
                    _uiElement.IsActive = false;
                }
            }
        }

        public override void OnComponentRemove()
        {
            if (_addedUIElement)
            {
                var screen = GameInstance.ScreenManager.CurrentScreen;
                if (screen is OverworldScreen)
                    ((OverworldScreen)screen).RemoveUIElement(_uiElement);
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

