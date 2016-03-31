using System.Linq;
using Microsoft.Xna.Framework;
using Pokemon3D.GameCore;
using Pokemon3D.Screens.Transitions;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.GUI;
using Pokemon3D.Rendering.Compositor;
using Pokemon3D.DataModel.GameCore;
using System.Collections.Generic;
using Pokemon3D.GameModes;
using Pokemon3D.Screens.Overworld;

namespace Pokemon3D.Screens.GameModeLoading
{
    class GameModeLoadingScreen : GameObject, Screen
    {
        // todo: move this somewhere else...
        private static readonly Dictionary<ShadowQuality, int> ShadowMapSizeForQuality = new Dictionary<ShadowQuality, int>
        {
            { ShadowQuality.Small, 512 },
            { ShadowQuality.Medium, 1024 },
            { ShadowQuality.Large, 2048 }
        };

        private bool _loadingFinished;
        private Sprite _pokeBallSprite;
        private SpriteText _loadingText;

        private World _world;

        public void OnOpening(object enterInformation)
        {
            _pokeBallSprite = new Sprite(Game.Content.Load<Texture2D>(ResourceNames.Textures.Pokeball))
            {
                Position = new Vector2(Game.ScreenBounds.Width, Game.ScreenBounds.Height)*0.5f
            };

            _loadingText = new SpriteText(Game.Content.Load<SpriteFont>(ResourceNames.Fonts.BigFont), "@Loading...");
            _loadingText.SetTargetRectangle(new Rectangle(0, 100, Game.ScreenBounds.Width, 100));
            _loadingText.HorizontalAlignment = HorizontalAlignment.Center;
            _loadingText.VerticalAlignment = VerticalAlignment.Top;

            var renderer = Game.Renderer;
            renderer.AddPostProcessingStep(new HorizontalBlurPostProcessingStep());
            renderer.AddPostProcessingStep(new VerticalBlurPostProcessingStep());
            renderer.EnablePostProcessing = false;
            renderer.Light.Direction = new Vector3(-1.5f, -1.0f, -0.5f);
            renderer.Light.AmbientIntensity = 0.5f;
            renderer.Light.DiffuseIntensity = 0.8f;
            renderer.AmbientLight = new Vector4(0.7f, 0.5f, 0.5f, 1.0f);

            var gameModes = Game.GameModeManager.GetGameModeInfos();
            Game.ActiveGameMode = Game.GameModeManager.CreateGameMode(gameModes.First(), Game);

            _loadingFinished = false;
            _world = new World();
            _world.StartNewGameAsync(() => _loadingFinished = true);
        }

        public void OnDraw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);

            Game.SpriteBatch.Begin();
            _loadingText.Draw(Game.SpriteBatch);
            _pokeBallSprite.Draw(Game.SpriteBatch);
            Game.SpriteBatch.End();
        }

        public void OnUpdate(float elapsedTime)
        {
            _pokeBallSprite.Rotation += elapsedTime * MathHelper.Pi;

            if (_loadingFinished)
            {
                Game.ScreenManager.SetScreen(typeof(OverworldScreen), typeof(SlideTransition), _world);
            }
        }
        
        public void OnClosing()
        {

        }
    }
}
