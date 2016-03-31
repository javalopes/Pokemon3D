using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes;
using Pokemon3D.Rendering.Compositor;
using System;
using Pokemon3D.Screens.Transitions;
using Pokemon3D.DataModel.Savegame;
using Pokemon3D.DataModel.Savegame.Pokemon;
using Pokemon3D.DataModel.Savegame.Inventory;
using Pokemon3D.DataModel.Pokemon;
using Pokemon3D.Screens.MainMenu;

namespace Pokemon3D.Screens.Overworld
{
    class OverworldScreen : GameObject, Screen
    {
        private World _world;

        private SpriteFont _debugSpriteFont;
        private bool _showRenderStatistics;

        private bool _isLoaded;

        public void OnOpening(object enterInformation)
        {
            if (!_isLoaded)
            {
                _world = enterInformation as World;
                if (_world == null) throw new InvalidOperationException("Did not receive loaded data.");

                _debugSpriteFont = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.DebugFont);

                CreateTempSave();

                _isLoaded = true;
            }
        }

        private void CreateTempSave()
        {
            // creates a temporary save until we get full load/save management done.

            var dataModel = new SaveFileModel
            {
                PlayerData = new PlayerModel(),
                GameMode = "TestGM",
                Items = new[]
                {
                    new InventoryItemModel
                    {
                        Amount = 2,
                        Id = "Potion"
                    }
                },
                Pokedexes = new[]
                {
                    new PokedexSaveModel
                    {
                        PokedexId = "National",
                        Entries = new[]
                        {
                            new PokedexEntrySaveModel
                            {
                                EntryType = PokedexEntryType.Caught,
                                Forms = new[]
                                {
                                    "Default",
                                    "Shiny"
                                },
                                Id = "Bulbasaur"
                            }
                        }
                    }
                },
                Pokemon = new[]
                {
                    new PokemonSaveModel
                    {
                        Id = "Charizard",
                        HP = 1,
                        IVs = new PokemonStatSetModel
                        {
                            HP = 21,
                            Atk = 20,
                            Def = 11,
                            SpAtk = 15,
                            SpDef = 30,
                            Speed = 1
                        },
                        EVs = new PokemonStatSetModel
                        {
                            HP = 50,
                            Atk = 255,
                            Def = 128,
                            SpAtk = 255,
                            SpDef = 255,
                            Speed = 0
                        },
                        Experience = 10000
                    },
                    new PokemonSaveModel
                    {
                        Id = "Bulbasaur",
                        HP = 5,
                        IVs = new PokemonStatSetModel
                        {
                            HP = 10
                        },
                        EVs = new PokemonStatSetModel
                        {
                            HP = 0
                        }
                    },
                    new PokemonSaveModel
                    {
                        Id = "Charizard",
                        HP = 12,
                        IVs = new PokemonStatSetModel
                        {
                            HP = 10
                        },
                        EVs = new PokemonStatSetModel
                        {
                            HP = 0
                        },
                        IsShiny = true
                    },
                    new PokemonSaveModel
                    {
                        Id = "Bulbasaur",
                        HP = 2,
                        IVs = new PokemonStatSetModel
                        {
                            HP = 10
                        },
                        EVs = new PokemonStatSetModel
                        {
                            HP = 0
                        }
                    },
                    new PokemonSaveModel
                    {
                        Id = "Bulbasaur",
                        HP = 11,
                        IVs = new PokemonStatSetModel
                        {
                            HP = 10
                        },
                        EVs = new PokemonStatSetModel
                        {
                            HP = 0
                        },
                        IsShiny = true
                    },
                    new PokemonSaveModel
                    {
                        Id = "Bulbasaur",
                        HP = 8,
                        IVs = new PokemonStatSetModel
                        {
                            HP = 10
                        },
                        EVs = new PokemonStatSetModel
                        {
                            HP = 0
                        }
                    }
                }
            };

            Game.LoadedSave = new SaveGame(dataModel);
            Game.LoadedSave.Load(Game.ActiveGameMode);
        }

        public void OnUpdate(float elapsedTime)
        {
            _world.Update(elapsedTime);
            Game.EntitySystem.Update(elapsedTime);

            if (Game.InputSystem.Keyboard.IsKeyDown(Keys.Escape))
            {
                Game.ScreenManager.SetScreen(typeof(MainMenuScreen), typeof(BlendTransition));
            }

            if (Game.InputSystem.Keyboard.IsKeyDownOnce(Keys.L))
            {
                Game.Renderer.EnablePostProcessing = !Game.Renderer.EnablePostProcessing;
            }

            if (Game.InputSystem.Keyboard.IsKeyDownOnce(Keys.F12))
            {
                _showRenderStatistics = !_showRenderStatistics;
            }

            if (Game.InputSystem.Keyboard.IsKeyDownOnce(Keys.X) || Game.InputSystem.GamePad.IsButtonDownOnce(Buttons.X))
            {
                // Game.ScreenManager.PushScreen(typeof(OverlayScreen));
                // Game.ScreenManager.PushScreen(typeof(TabletScreen));
                // Game.ScreenManager.PushScreen(typeof(PokemonTableScreen));
            }
        }

        public void OnDraw(GameTime gameTime)
        {
            Game.CollisionManager.Draw(_world.Player.Camera);
            if (_showRenderStatistics) DrawRenderStatsitics();
        }

        private void DrawRenderStatsitics()
        {
            var renderStatistics = RenderStatistics.Instance;

            const int spacing = 5;
            var elementHeight = _debugSpriteFont.LineSpacing + spacing;
            var height = elementHeight * 4 + spacing;
            const int width = 180;

            var startPosition = new Vector2(Game.ScreenBounds.Width - width, Game.ScreenBounds.Height - height);

            Game.SpriteBatch.Begin();
            Game.ShapeRenderer.DrawRectangle((int)startPosition.X,
                (int)startPosition.Y,
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
