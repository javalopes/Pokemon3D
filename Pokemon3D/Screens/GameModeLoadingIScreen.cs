using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Common.Localization;
using Pokemon3D.Content;
using Pokemon3D.Entities;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Rendering.UI.Animations;
using Pokemon3D.Rendering.UI.Controls;
using Pokemon3D.Screens.Transitions;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Screens
{
    internal class GameModeLoadingIScreen : IScreenWithOverlays
    {
        private UiElement _pokeballSprite;
        private World _world;
        private UiOverlay _overlay;

        public override void OnOpening(object enterInformation)
        {
            var translation = GameInstance.GetService<ITranslationProvider>();
            var contentManager = GameInstance.GetService<ContentManager>();

            var window = GameInstance.GetService<Window>();

            _overlay = AddOverlay(new UiOverlay());
            _pokeballSprite = _overlay.AddElement(new Image(contentManager.Load<Texture2D>(ResourceNames.Textures.Pokeball)));
            _pokeballSprite.SetPosition(new Vector2(window.ScreenBounds.Width, window.ScreenBounds.Height) * 0.5f);
            _pokeballSprite.SetOriginPercentage(new Vector2(0.5f));
            _pokeballSprite.EnterAnimation = new UiScaleAnimation(0.5f, Vector2.Zero, Vector2.One);
            _pokeballSprite.LeaveAnimation = new UiScaleAnimation(0.5f, Vector2.One, Vector2.Zero);
            _pokeballSprite.Scale = Vector2.Zero;
            _pokeballSprite.AddCustomAnimation("Rotating", new UiRotationAnimation(0.5f, 0.0f, MathHelper.TwoPi), true);

            var loadingText = _overlay.AddElement(new StaticText(contentManager.Load<SpriteFont>(ResourceNames.Fonts.BigFont), translation.CreateValue("System", "GameLoadingMessage")));
            loadingText.SetPosition(new Vector2(window.ScreenBounds.Width * 0.5f, 400));
            loadingText.SetOriginPercentage(new Vector2(0.5f, 0.0f));
            loadingText.EnterAnimation = new UiAlphaAnimation(0.4f, 0.0f, 1.0f);
            loadingText.LeaveAnimation = new UiAlphaAnimation(0.4f, 1.0f, 0.0f);
            loadingText.Alpha = 0.0f;

            _overlay.Showed += OverlayOnShowed;
            _overlay.Hidden += OnHidden;
            _overlay.Show();
        }

        private void OnHidden()
        {
            GameInstance.GetService<ScreenManager>().SetScreen(typeof(OverworldIScreen), typeof(ShrinkOldTransition), _world);
        }

        private void OverlayOnShowed()
        {
            _pokeballSprite.PlayCustomAnimation("Rotating");
            StartCreateNewGame();
        }

        public override void OnLateDraw(GameTime gameTime)
        {
            GameInstance.GetService<GraphicsDevice>().Clear(Color.Black);
            base.OnLateDraw(gameTime);
        }

        private void StartCreateNewGame()
        {
            var gameModeManager = GameInstance.GetService<GameModeManager>();
            var gameModes = gameModeManager.GetGameModeInfos();
            gameModeManager.LoadAndSetGameMode(gameModes.First(), GameInstance);

            _world = new World();
            _world.StartNewGameAsync(() => _overlay.Hide());
        }
    }
}
