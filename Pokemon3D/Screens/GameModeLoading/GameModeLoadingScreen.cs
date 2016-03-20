using System.Linq;
using Microsoft.Xna.Framework;
using Pokemon3D.GameCore;
using Pokemon3D.UI.Transitions;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.GUI;
using System.Windows.Threading;
using Pokemon3D.Rendering.Compositor;
using Pokemon3D.DataModel.GameCore;
using System.Collections.Generic;
using Pokemon3D.GameModes.Maps;
using Pokemon3D.Rendering;
using Pokemon3D.DataModel.GameMode.Map;
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

        private Stopwatch _sw;
        private bool _loadingFinished;

        private Sprite _pokeBallSprite;
        private SpriteText _loadingText;

        private Dispatcher _dispatcher;

        private GameModeLoadingResult _result;

        public void OnOpening(object enterInformation)
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _result = new GameModeLoadingResult();

            _pokeBallSprite = new Sprite(Game.Content.Load<Texture2D>(ResourceNames.Textures.Pokeball));
            _pokeBallSprite.Position = new Vector2(Game.ScreenBounds.Width, Game.ScreenBounds.Height) * 0.5f;

            _loadingText = new SpriteText(Game.Content.Load<SpriteFont>(ResourceNames.Fonts.BigFont), "@Loading...");
            _loadingText.SetTargetRectangle(new Rectangle(0, 100, Game.ScreenBounds.Width, 100));
            _loadingText.HorizontalAlignment = HorizontalAlignment.Center;
            _loadingText.VerticalAlignment = VerticalAlignment.Top;

            var gameModes = Game.GameModeManager.GetGameModeInfos();
            Game.ActiveGameMode = Game.GameModeManager.CreateGameMode(gameModes.First(), Game);
            _loadingFinished = false;
            _sw = Stopwatch.StartNew();
            Game.ActiveGameMode.PreloadAsync(ContinueLoadMap);
        }

        private void ContinueLoadMap()
        {

            var settings = new RenderSettings
            {
                EnableShadows = Game.GameConfig.ShadowsEnabled,
                ShadowMapSize = ShadowMapSizeForQuality[Game.GameConfig.ShadowQuality],
                EnableSoftShadows = Game.GameConfig.SoftShadows
            };

            WindowsSceneEffect effect = null;
            _dispatcher.Invoke(() => { effect = new WindowsSceneEffect(Game.Content); });

            _result.SceneRenderer = SceneRendererFactory.Create(Game, effect, settings);
            _result.SceneRenderer.AddPostProcessingStep(new HorizontalBlurPostProcessingStep());
            _result.SceneRenderer.AddPostProcessingStep(new VerticalBlurPostProcessingStep());
            _result.SceneRenderer.EnablePostProcessing = false;

            _result.Scene = new Scene(Game)
            {
                Light =
                {
                    Direction = new Vector3(-1.5f, -1.0f, -0.5f),
                    AmbientIntensity = 0.5f,
                    DiffuseIntensity = 0.8f
                },
                AmbientLight = new Vector4(0.7f, 0.5f, 0.5f, 1.0f)
            };

            Game.ActiveGameMode.MapManager.LoadMapAsync(Game.ActiveGameMode.GameModeInfo.StartMap, FinishedLoadingMapModel);
        }

        private void FinishedLoadingMapModel(MapModel mapModel)
        {
            _result.Map = new Map(Game.ActiveGameMode, mapModel, _result.Scene);
            _result.Player = new Player(_result.Scene);

            _result.Map.AddEntity(new NPC(_result.Scene, new DataModel.GameMode.Map.NPCs.RandomNPCModel()
            {
                Name = "Testificate",
                Behaviour = DataModel.GameMode.Map.NPCs.NPCBehaviour.Roaming,
                Texture = new DataModel.TextureSourceModel()
                {
                    Source = "test",
                    Rectangle = null
                },
                Chance = 100,
                ScriptBinding = "somescript"
            }));
            _loadingFinished = true;
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
                _sw.Stop();
                Common.Diagnostics.GameLogger.Instance.Log(Common.Diagnostics.MessageType.Debug, "Loading time: " + _sw.ElapsedMilliseconds);
                Game.ScreenManager.SetScreen(typeof(OverworldScreen), typeof(SlideTransition), _result);
            }
        }
        
        public void OnClosing()
        {

        }
    }
}
