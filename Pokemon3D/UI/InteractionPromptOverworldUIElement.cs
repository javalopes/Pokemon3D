using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Common.Input;
using Pokemon3D.Common.Localization;
using Pokemon3D.Content;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Shapes;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Rendering.UI.Controls;
using Pokemon3D.ScriptPipeline;

namespace Pokemon3D.UI
{
    class InteractionPromptOverworldUiElement : UiOverlay
    {
        private readonly Vector3 _worldPosition;

        private float _buttonPressed;
        private ColoredPie _pie;

        public event Action<InteractionPromptOverworldUiElement> InteractionStarted;

        public InteractionPromptOverworldUiElement(Vector3 worldPosition, string message)
        {
            _worldPosition = worldPosition;

            var font = GameProvider.GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.LargeUIRegular);
            var fontSize = font.MeasureString(message);

            var  rect = AddElement(new ColoredRectangle(new Color(0, 0, 0, 180), new Rectangle(0, 0,(int) fontSize.X + 6, 30)));
            rect.SetPosition(new Vector2((int)(-fontSize.X / 2 - 3), 0));

            var messageText = AddElement(new StaticText(font, LocalizedValue.Static(message)));
            messageText.SetPosition(new Vector2(-(fontSize.X / 2f), 0));

            var ellipse = AddElement(new ColoredEllipse(new Color(64, 64, 64, 180), new Ellipse(0, 0, 48, 48)));
            ellipse.SetPosition(new Vector2(-24, 28));

            var secondEllipse = AddElement(new ColoredEllipse(new Color(64, 200, 64), new Ellipse(0, 0, 32, 32)));
            secondEllipse.SetPosition(new Vector2(-24 + 8, +28 + 8));

            _pie = AddElement(new ColoredPie(Color.LightGray, new Ellipse(0, 0, 48, 48)));
            _pie.Angle = 0.0f;

            var actionText = AddElement(new Image(GameProvider.GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_A)));
            actionText.SetPosition(new Vector2(-24 + 8, +28 + 8));

            Showed += OnShowed;
            Hidden += OnHidden;
        }

        private void OnHidden()
        {
            _buttonPressed = 0f;
        }

        private void OnShowed()
        {
            var camera = GameProvider.GameInstance.GetService<SceneRenderer>().GetMainCamera();

            var position = camera.ProjectWorldToScreen(_worldPosition + new Vector3(0, 0.5f, 0));

            ApplyOffset(new Vector2(position.X, position.Y));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (GameProvider.GameInstance.GetService<InputSystem>().IsPressed(ActionNames.MenuAccept) &&
                ScriptPipelineManager.ActiveProcessorCount == 0) // only update these if this are no scripts running.
            {
                _buttonPressed += gameTime.GetSeconds() * 1.5f;
                if (_buttonPressed >= 1f)
                {
                    _buttonPressed = 0f;
                    InteractionStarted?.Invoke(this);
                    Hide();
                }
            }
            else
            {
                if (_buttonPressed > 0f)
                {
                    _buttonPressed -= 0.1f;
                    if (_buttonPressed <= 0f)
                    {
                        _buttonPressed = 0f;
                    }
                }
            }

            _pie.Angle = MathHelper.TwoPi * _buttonPressed;
        }
    }
}
