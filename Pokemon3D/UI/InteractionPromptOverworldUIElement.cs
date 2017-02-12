using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Common.Localization;
using Pokemon3D.Content;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Shapes;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Rendering.UI.Controls;
using Pokemon3D.ScriptPipeline;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.UI
{
    class InteractionPromptOverworldUiElement : UiOverlay
    {
        private readonly Vector3 _worldPosition;

        private float _buttonPressed;
        private readonly ColoredPie _pie;

        public event Action<InteractionPromptOverworldUiElement> InteractionStarted;

        public InteractionPromptOverworldUiElement(Vector3 worldPosition, string message)
        {
            _worldPosition = worldPosition;

            var font = IGameInstance.GetService<ContentManager>().Load<SpriteFont>(ResourceNames.Fonts.LargeUIRegular);
            var fontSize = font.MeasureString(message);

            var  rect = AddElement(new ColoredRectangle(new Color(0, 0, 0, 180), new Rectangle(0, 0,(int) fontSize.X + 6, 30)));
            rect.SetPosition(new Vector2((int)(-fontSize.X / 2 - 3), 0));

            var messageText = AddElement(new StaticText(font, LocalizedValue.Static(message)));
            messageText.SetPosition(new Vector2(-(fontSize.X / 2f), 0));

            var ellipse = AddElement(new ColoredEllipse(new Color(64, 64, 64, 180), new Ellipse(0, 0, 40, 40)));
            const int verticalOffset = 64;

            ellipse.SetPosition(new Vector2(0, verticalOffset));

            var secondEllipse = AddElement(new ColoredEllipse(new Color(64, 200, 64), new Ellipse(0, 0, 32, 32)));
            secondEllipse.SetPosition(new Vector2(0, +verticalOffset));

            _pie = AddElement(new ColoredPie(Color.LightGray, new Ellipse(0, 0, 40, 40)));
            _pie.SetPosition(new Vector2(0, +verticalOffset));
            _pie.Angle = 0.0f;

            var actionText = AddElement(new Image(IGameInstance.GetService<ContentManager>().Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_A)));
            actionText.SetPosition(new Vector2(0.0f, verticalOffset));
            actionText.SetOriginPercentage(new Vector2(0.5f, 0.5f));

            Showed += OnShowed;
            Hidden += OnHidden;
        }

        private void OnHidden()
        {
            _buttonPressed = 0f;
        }

        private void OnShowed()
        {
            var camera = IGameInstance.GetService<ISceneRenderer>().GetMainCamera();

            var position = camera.ProjectWorldToScreen(_worldPosition + new Vector3(0, 0.5f, 0));

            ApplyOffset(new Vector2(position.X, position.Y));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IGameInstance.GetService<InputSystem.InputSystem>().IsPressed(ActionNames.MenuAccept) &&
                IGameInstance.GetService<ScriptPipelineManager>().ActiveProcessorCount == 0) // only update these if this are no scripts running.
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
