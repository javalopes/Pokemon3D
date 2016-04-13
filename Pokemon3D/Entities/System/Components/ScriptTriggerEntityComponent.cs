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
        private string _message;

        private InteractionPromptOverworldUIElement _uiElement;
        private bool _addedUIElement = false;

        private Collider _collider;

        public ScriptTriggerEntityComponent(EntityComponentDataCreationStruct parameters)
            : base(parameters)
        {
            _script = GetDataOrDefault("Script", "");
            _trigger = GetDataOrDefault("Trigger", "Interaction");
            _message = GetDataOrDefault("Message", "Interact");

            _collider = Collider.CreateBoundingBox(Parent.Scale, null);
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
            uiElement.Message = "It worked!";
        }
    }
}

