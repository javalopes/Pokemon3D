using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes;
using Pokemon3D.GameModes.Maps;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Compositor;
using System;
using System.Windows.Threading;
using Pokemon3D.UI.Transitions;
using Pokemon3D.DataModel.Savegame;
using Pokemon3D.DataModel.Savegame.Pokemon;
using Pokemon3D.DataModel.Savegame.Inventory;
using Pokemon3D.DataModel.Pokemon;

namespace Pokemon3D.UI.Screens
{
    class OverworldScreen : GameObject, Screen
    {
        private GameMode _gameMode;
        private Map _currentMap;
        private SceneRenderer _renderer;
        private Scene _scene;
        private Player _player;

        private Dispatcher _dispatcher;

        private SpriteFont _debugSpriteFont;
        private bool _showRenderStatistics;

        private bool _isLoaded = false;

        public void OnOpening(object enterInformation)
        {
            if (!_isLoaded)
            {
                _gameMode = Game.ActiveGameMode;
                _dispatcher = Dispatcher.CurrentDispatcher;

                var loadingResult = enterInformation as GameModeLoadingResult;
                if (loadingResult == null) throw new InvalidOperationException("Did not receive loaded data.");

                _renderer = loadingResult.SceneRenderer;
                _scene = loadingResult.Scene;
                _player = loadingResult.Player;

                _debugSpriteFont = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.DebugFont);

                CreateTempSave();

                _isLoaded = true;
            }
        }

        private void CreateTempSave()
        {
            // creates a temporary save until we get full load/save management done.

            var dataModel = new SaveFileModel();
            dataModel.PlayerData = new PlayerModel();
            dataModel.GameMode = "TestGM";
            dataModel.Items = new InventoryItemModel[] { };
            dataModel.Pokedex = new PokedexSaveModel();
            dataModel.Pokemon = new PokemonSaveModel[] 
            {
                new PokemonSaveModel()
                {
                    Id = "Charizard",
                    HP = 1,
                    IVs = new PokemonStatSetModel()
                    {
                        HP = 10
                    },
                    EVs = new PokemonStatSetModel()
                    {
                        HP = 0
                    }
                },
                new PokemonSaveModel()
                {
                    Id = "Bulbasaur",
                    HP = 5,
                    IVs = new PokemonStatSetModel()
                    {
                        HP = 10
                    },
                    EVs = new PokemonStatSetModel()
                    {
                        HP = 0
                    }
                },
                new PokemonSaveModel()
                {
                    Id = "Charizard",
                    HP = 12,
                    IVs = new PokemonStatSetModel()
                    {
                        HP = 10
                    },
                    EVs = new PokemonStatSetModel()
                    {
                        HP = 0
                    },
                    IsShiny = true
                },new PokemonSaveModel()
                {
                    Id = "Bulbasaur",
                    HP = 2,
                    IVs = new PokemonStatSetModel()
                    {
                        HP = 10
                    },
                    EVs = new PokemonStatSetModel()
                    {
                        HP = 0
                    }
                },
                new PokemonSaveModel()
                {
                    Id = "Bulbasaur",
                    HP = 11,
                    IVs = new PokemonStatSetModel()
                    {
                        HP = 10
                    },
                    EVs = new PokemonStatSetModel()
                    {
                        HP = 0
                    },
                    IsShiny = true
                },
                new PokemonSaveModel()
                {
                    Id = "Bulbasaur",
                    HP = 8,
                    IVs = new PokemonStatSetModel()
                    {
                        HP = 10
                    },
                    EVs = new PokemonStatSetModel()
                    {
                        HP = 0
                    }
                }
            };

            Game.LoadedSave = new SaveGame(dataModel);
            Game.LoadedSave.Load(_gameMode);
        }

        public void OnUpdate(float elapsedTime)
        {
            _player.Update(elapsedTime);
            _currentMap?.Update(elapsedTime);
            _scene.Update(elapsedTime);

            if (Game.InputSystem.Keyboard.IsKeyDown(Keys.Escape))
            {
                Game.ScreenManager.SetScreen(typeof(MainMenuScreen), typeof(BlendTransition));
            }

            if (Game.InputSystem.Keyboard.IsKeyDownOnce(Keys.L))
            {
                _renderer.EnablePostProcessing = !_renderer.EnablePostProcessing;
            }

            if (Game.InputSystem.Keyboard.IsKeyDownOnce(Keys.F12))
            {
                _showRenderStatistics = !_showRenderStatistics;
            }

            if (Game.InputSystem.Keyboard.IsKeyDownOnce(Keys.V))
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

            if (Game.InputSystem.GamePad.IsButtonDownOnce(Buttons.X))
            {
                Game.ScreenManager.SetScreen(typeof(OverlayScreen), enterInformation: this);
            }

            if (Game.InputSystem.Keyboard.IsKeyDownOnce(Keys.F10))
            {
                _player.MovementMode = PlayerMovementMode.GodMode;
                Game.NotificationBar.PushNotification(NotificationKind.Information, "Enabled God Mode");
            }
        }

        public void OnDraw(GameTime gameTime)
        {
            _currentMap?.RenderPreparations(_player.Camera);
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
