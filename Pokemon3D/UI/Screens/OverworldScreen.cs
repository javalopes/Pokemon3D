using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.DataModel.GameCore;
using Pokemon3D.DataModel.GameMode.Map;
using Pokemon3D.FileSystem.Requests;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes;
using Pokemon3D.GameModes.Maps;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Compositor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace Pokemon3D.UI.Screens
{
    class OverworldScreen : GameObject, Screen
    {
        // todo: move this somewhere else...
        private static readonly Dictionary<ShadowQuality, int> ShadowMapSizeForQuality = new Dictionary<ShadowQuality, int>
        {
            { ShadowQuality.Small, 512 },
            { ShadowQuality.Medium, 1024 },
            { ShadowQuality.Large, 2048 }
        }; 

        private GameMode _gameMode;
        private Map _currentMap;
        private SceneRenderer _renderer;
        private Scene _scene;
        private Player _player;

        private Dispatcher _dispatcher;

        private SpriteFont _debugSpriteFont;
        private bool _showRenderStatistics;
        
        public void OnOpening(object enterInformation)
        {
            _gameMode = Game.ActiveGameMode;
            _dispatcher = Dispatcher.CurrentDispatcher;

            var settings = new RenderSettings
            {
                EnableShadows = Game.GameConfig.ShadowsEnabled,
                ShadowMapSize = ShadowMapSizeForQuality[Game.GameConfig.ShadowQuality],
                EnableSoftShadows = Game.GameConfig.SoftShadows
            };
            _renderer = SceneRendererFactory.Create(Game, new WindowsSceneEffect(Game.Content), settings);

            _scene = new Scene(Game)
            {
                Light =
                {
                    Direction = new Vector3(-1.5f, -1.0f, -0.5f),
                    AmbientIntensity = 0.5f,
                    DiffuseIntensity = 0.8f
                },
                AmbientLight = new Vector4(0.7f, 0.5f, 0.5f, 1.0f)
            };

            _player = new Player(_scene);

            _debugSpriteFont = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.DebugFont);

            LoadMap();
        }

        private void LoadMap()
        {
            var request = _gameMode.MapManager.CreateDataRequest(_gameMode.GameModeInfo.StartMap);
            request.Finished += FinishedLoadingMapModel;
            request.StartThreaded();
        }

        private void FinishedLoadingMapModel(object sender, EventArgs e)
        {
            var request = (DataModelRequest<MapModel>)sender;

            if (request.Status == DataRequestStatus.Complete)
            {
                _dispatcher.Invoke(() =>
                {
                    _currentMap = new Map(_gameMode, request.ResultModel, _scene, Game.Resources);
                });
            }
            else
            {
                Common.Diagnostics.GameLogger.Instance.Log(Common.Diagnostics.MessageType.Error, "Map (" + request.DataPath + ") loading failed! See next message for exception details.");
                Common.Diagnostics.GameLogger.Instance.Log(request.RequestException);
            }
        }

        public void OnUpdate(float elapsedTime)
        {
            _player.Update(elapsedTime);
            _currentMap?.Update(elapsedTime);
            _scene.Update(elapsedTime);

            if (Game.Keyboard.IsKeyDown(Keys.Escape))
            {
                Game.ScreenManager.SetScreen(typeof(MainMenuScreen));
            }

            if (Game.Keyboard.IsKeyDownOnce(Keys.F12))
            {
                _showRenderStatistics = !_showRenderStatistics;
            }
            if (Game.Keyboard.IsKeyDownOnce(Keys.F11))
            {
                _renderer.RenderSettings.EnableShadows = !_renderer.RenderSettings.EnableShadows;
                if (_renderer.RenderSettings.EnableShadows)
                {
                    Game.NotificationBar.PushNotification(NotificationKind.Information, "Enabled Shadows");
                }
                else
                {
                    Game.NotificationBar.PushNotification(NotificationKind.Information, "Disabled Shadows");
                }
            }

            if (Game.Keyboard.IsKeyDownOnce(Keys.V))
            {
                if (_player.MovementMode == PlayerMovementMode.GodMode)
                {
                    Game.NotificationBar.PushNotification(NotificationKind.Information, "Disabled God Mode");
                }
                if (_player.MovementMode == PlayerMovementMode.FirstPerson)
                {
                    _player.MovementMode = PlayerMovementMode.ThirdPerson;
                }
                else
                {
                    _player.MovementMode = PlayerMovementMode.FirstPerson;
                }
            }

            if (Game.Keyboard.IsKeyDownOnce(Keys.F10))
            {
                _player.MovementMode = PlayerMovementMode.GodMode;
                Game.NotificationBar.PushNotification(NotificationKind.Information, "Enabled God Mode");
            }
        }

        public void OnDraw(GameTime gameTime)
        {
            _renderer.Draw(_scene);
            Game.CollisionManager.Draw(_player.Camera);

            if (_showRenderStatistics) DrawRenderStatsitics();
        }

        private void DrawRenderStatsitics()
        {
            var renderStatistics = RenderStatistics.Instance;

            const int spacing = 5;
            var elementHeight = _debugSpriteFont.LineSpacing + spacing;
            var height = elementHeight*4 + spacing;
            const int width = 180;

            var startPosition = new Vector2(Game.ScreenBounds.Width - width, Game.ScreenBounds.Height - height);

            Game.SpriteBatch.Begin();
            Game.ShapeRenderer.DrawFilledRectangle((int) startPosition.X,
                (int) startPosition.Y,
                width,
                height,
                Color.DarkGreen);

            startPosition.X += spacing;
            startPosition.Y += spacing;
            Game.SpriteBatch.DrawString(_debugSpriteFont,
                string.Format("Average DrawTime[ms]: {0:0.00}", renderStatistics.AverageDrawTime), startPosition, Color.White);
            startPosition.Y += elementHeight;
            Game.SpriteBatch.DrawString(_debugSpriteFont, string.Format("Total Drawcalls: {0}", renderStatistics.DrawCalls),
                startPosition, Color.White);
            Game.SpriteBatch.End();
        }

        public void OnClosing()
        {
        }
        
    }
}
