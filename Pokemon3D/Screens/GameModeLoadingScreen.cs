using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Localization;
using Pokemon3D.Content;
using Pokemon3D.Entities;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Rendering.UI.Animations;
using Pokemon3D.Rendering.UI.Controls;
using Pokemon3D.Screens.Transitions;

namespace Pokemon3D.Screens
{
    internal class GameModeLoadingScreen : ScreenWithOverlays
    {
        private UiElement _pokeballSprite;
        private World _world;
        private UiOverlay _overlay;

        public override void OnOpening(object enterInformation)
        {
            var translation = GameProvider.GameInstance.GetService<TranslationProvider>();
            var contentManager = GameProvider.GameInstance.GetService<ContentManager>();

            _overlay = AddOverlay(new UiOverlay());
            _pokeballSprite = _overlay.AddElement(new Image(contentManager.Load<Texture2D>(ResourceNames.Textures.Pokeball)));
            _pokeballSprite.SetPosition(new Vector2(GameProvider.GameInstance.ScreenBounds.Width, GameProvider.GameInstance.ScreenBounds.Height) * 0.5f);
            _pokeballSprite.SetOriginPercentage(new Vector2(0.5f));
            _pokeballSprite.EnterAnimation = new UiScaleAnimation(0.5f, Vector2.Zero, Vector2.One);
            _pokeballSprite.LeaveAnimation = new UiScaleAnimation(0.5f, Vector2.One, Vector2.Zero);
            _pokeballSprite.Scale = Vector2.Zero;
            _pokeballSprite.AddCustomAnimation("Rotating", new UiRotationAnimation(0.5f, 0.0f, MathHelper.TwoPi), true);

            var loadingText = _overlay.AddElement(new StaticText(contentManager.Load<SpriteFont>(ResourceNames.Fonts.BigFont), translation.CreateValue("System", "GameLoadingMessage")));
            loadingText.SetPosition(new Vector2(GameProvider.GameInstance.ScreenBounds.Width * 0.5f, 400));
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
            GameProvider.GameInstance.GetService<ScreenManager>().SetScreen(typeof(OverworldScreen), typeof(ShrinkOldTransition), _world);
        }

        private void OverlayOnShowed()
        {
            _pokeballSprite.PlayCustomAnimation("Rotating");
            StartCreateNewGame();
        }

        public override void OnLateDraw(GameTime gameTime)
        {
            GameProvider.GameInstance.GetService<GraphicsDevice>().Clear(Color.Black);
            base.OnLateDraw(gameTime);
        }

        private void StartCreateNewGame()
        {
            var gameModeManager = GameProvider.GameInstance.GetService<GameModeManager>();
            var gameModes = gameModeManager.GetGameModeInfos();
            gameModeManager.LoadAndSetGameMode(gameModes.First(), GameProvider.GameInstance);

            _world = new World();
            _world.StartNewGameAsync(() => _overlay.Hide());
        }
    }
}
