using System.Linq;
using Microsoft.Xna.Framework;
using Pokemon3D.Screens.Transitions;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.GUI;
using Pokemon3D.Rendering.Compositor;
using Pokemon3D.DataModel.GameCore;
using System.Collections.Generic;
using Pokemon3D.Entities;
using Pokemon3D.Screens.Overworld;
using Pokemon3D.Common.Extensions;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Screens.GameModeLoading
{
    class GameModeLoadingScreen : Screen
    {

        private bool _loadingFinished;
        private Sprite _pokeBallSprite;
        private SpriteText _loadingText;

        private World _world;

        public void OnOpening(object enterInformation)
        {
            _pokeBallSprite = new Sprite(GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.Pokeball))
            {
                Position = new Vector2(GameInstance.ScreenBounds.Width, GameInstance.ScreenBounds.Height) * 0.5f
            };

            _loadingText = new SpriteText(GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.BigFont), "@Loading...");
            _loadingText.SetTargetRectangle(new Rectangle(0, 100, GameInstance.ScreenBounds.Width, 100));
            _loadingText.HorizontalAlignment = HorizontalAlignment.Center;
            _loadingText.VerticalAlignment = VerticalAlignment.Top;

            var renderer = GameInstance.SceneRenderer;
            renderer.AddPostProcessingStep(new HorizontalBlurPostProcessingStep());
            renderer.AddPostProcessingStep(new VerticalBlurPostProcessingStep());
            renderer.EnablePostProcessing = false;
            renderer.Light.Direction = new Vector3(-1.5f, -1.0f, -0.5f);
            renderer.Light.AmbientIntensity = 0.5f;
            renderer.Light.DiffuseIntensity = 0.8f;
            renderer.AmbientLight = new Vector4(0.7f, 0.5f, 0.5f, 1.0f);

            var gameModes = GameInstance.GameModeManager.GetGameModeInfos();
            GameInstance.ActiveGameMode = GameInstance.GameModeManager.CreateGameMode(gameModes.First(), GameInstance);

            _loadingFinished = false;
            _world = new World();
            _world.StartNewGameAsync(() => _loadingFinished = true);
        }

        public void OnLateDraw(GameTime gameTime)
        {
            GameInstance.GraphicsDevice.Clear(Color.Black);

            GameInstance.SpriteBatch.Begin();
            _loadingText.Draw(GameInstance.SpriteBatch);
            _pokeBallSprite.Draw(GameInstance.SpriteBatch);
            GameInstance.SpriteBatch.End();
        }

        public void OnEarlyDraw(GameTime gameTime)
        {
        }

        public void OnUpdate(GameTime gameTime)
        {
            _pokeBallSprite.Rotation += gameTime.GetSeconds() * MathHelper.Pi;

            if (_loadingFinished)
            {
                _world.ActivateNewEntities();
                GameInstance.ScreenManager.SetScreen(typeof(OverworldScreen), typeof(SlideTransition), _world);
            }
        }

        public void OnClosing()
        {

        }
    }
}
