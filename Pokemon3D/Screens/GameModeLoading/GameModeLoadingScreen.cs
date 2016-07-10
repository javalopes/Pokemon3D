using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Pokemon3D.Screens.Transitions;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Entities;
using Pokemon3D.Rendering.Compositor;
using Pokemon3D.Screens.Overworld;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Rendering.UI.Animations;
using Pokemon3D.Rendering.UI.Controls;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Screens.GameModeLoading
{
    class GameModeLoadingScreen : Screen
    {
        private bool _loadingFinished;

        private UiOverlay _overlay;

        private World _world;

        public void OnOpening(object enterInformation)
        {
            _overlay = new UiOverlay();
            var pokeballSprite = _overlay.AddElement(new Image(GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.Pokeball)));
            pokeballSprite.SetPosition(new Vector2(GameInstance.ScreenBounds.Width, GameInstance.ScreenBounds.Height) * 0.5f);
            pokeballSprite.SetOriginPercentage(new Vector2(0.5f));
            pokeballSprite.EnterAnimation =  new UiScaleAnimation(0.5f, Vector2.Zero, Vector2.One);
            pokeballSprite.Scale = Vector2.Zero;

            var loadingText = _overlay.AddElement(new StaticText(GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.BigFont), "@Loading..."));
            loadingText.SetPosition(new Vector2(GameInstance.ScreenBounds.Width * 0.5f, 400));
            loadingText.SetOriginPercentage(new Vector2(0.5f, 0.0f));
            loadingText.EnterAnimation = new UiAlphaAnimation(0.4f, 0.0f, 1.0f);
            loadingText.Alpha = 0.0f;

            _overlay.Showed += OverlayOnShowed;
            _overlay.Show();
        }

        private void OverlayOnShowed()
        {
            StartCreateNewGame();
        }

        public void OnLateDraw(GameTime gameTime)
        {
            GameInstance.GraphicsDevice.Clear(Color.Black);

            _overlay.Draw(GameInstance.SpriteBatch);
        }

        public void OnEarlyDraw(GameTime gameTime)
        {
        }

        public void OnUpdate(GameTime gameTime)
        {
            _overlay.Update(gameTime);

            if (_loadingFinished)
            {
                _world.ActivateNewEntities();
                GameInstance.ScreenManager.SetScreen(typeof(OverworldScreen), typeof(SlideTransition), _world);
            }
        }

        public void OnClosing()
        {

        }

        private void StartCreateNewGame()
        {
            var renderer = GameInstance.SceneRenderer;
            renderer.AddPostProcessingStep(new HorizontalBlurPostProcessingStep());
            renderer.AddPostProcessingStep(new VerticalBlurPostProcessingStep());
            renderer.EnablePostProcessing = false;

            var mainLight = renderer.CreateDirectionalLight(new Vector3(-1.5f, -1.0f, -0.5f));
            mainLight.AmbientIntensity = 0.5f;
            mainLight.DiffuseIntensity = 0.8f;
            renderer.AmbientLight = new Vector4(0.7f, 0.5f, 0.5f, 1.0f);

            var gameModes = GameInstance.GameModeManager.GetGameModeInfos();
            GameInstance.ActiveGameMode = GameInstance.GameModeManager.CreateGameMode(gameModes.First(), GameInstance);

            _loadingFinished = false;
            _world = new World();
            _world.StartNewGameAsync(() => _loadingFinished = true);
        }
    }
}
