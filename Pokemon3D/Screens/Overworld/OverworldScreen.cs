﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Entities;
using Pokemon3D.Rendering.Compositor;
using System;
using Pokemon3D.Screens.Transitions;
using Pokemon3D.DataModel.Savegame;
using Pokemon3D.DataModel.Savegame.Pokemon;
using Pokemon3D.DataModel.Savegame.Inventory;
using Pokemon3D.DataModel.Pokemon;
using Pokemon3D.Screens.MainMenu;
using System.Collections.Generic;
using System.Linq;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Screens.Overworld
{
    class OverworldScreen : Screen
    {
        private World _world;

        public World ActiveWorld
        {
            get { return _world; }
        }

        private SpriteFont _debugSpriteFont;
        private bool _showRenderStatistics;

        private bool _isLoaded;

        public void OnOpening(object enterInformation)
        {
            if (!_isLoaded)
            {
                _world = enterInformation as World;
                if (_world == null) throw new InvalidOperationException("Did not receive loaded data.");

                _debugSpriteFont = GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.DebugFont);

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

            GameInstance.LoadedSave = new SaveGame(dataModel);
            GameInstance.LoadedSave.Load(GameInstance.ActiveGameMode);
        }

        public void OnUpdate(GameTime gameTime)
        {
            _world.Update(gameTime);

            lock (_uiElements)
                _uiElements.ForEach(e => { if (e.IsActive) e.Update(gameTime); });

            if (GameInstance.InputSystem.Keyboard.IsKeyDown(Keys.Escape))
            {
                GameInstance.ScreenManager.SetScreen(typeof(MainMenuScreen), typeof(BlendTransition));
            }

            if (GameInstance.InputSystem.Keyboard.IsKeyDownOnce(Keys.L))
            {
                GameInstance.Renderer.EnablePostProcessing = !GameInstance.Renderer.EnablePostProcessing;
            }

            if (GameInstance.InputSystem.Keyboard.IsKeyDownOnce(Keys.F12))
            {
                _showRenderStatistics = !_showRenderStatistics;
            }

            if (GameInstance.InputSystem.Keyboard.IsKeyDownOnce(Keys.X) || GameInstance.InputSystem.GamePad.IsButtonDownOnce(Buttons.X))
            {
                GameInstance.ScreenManager.SetScreen(typeof(GameMenu.GameMenuScreen));
            }
        }

        public void OnLateDraw(GameTime gameTime)
        {
            GameInstance.CollisionManager.Draw(_world.Player.Camera);
            if (_showRenderStatistics) DrawRenderStatsitics();

            if (_uiElements.Count > 0 && _uiElements.Any(e => e.IsActive))
            {
                GameInstance.SpriteBatch.Begin();

                lock (_uiElements)
                    _uiElements.ForEach(e => { if (e.IsActive) e.Draw(gameTime); });

                GameInstance.SpriteBatch.End();
            }
        }

        public void OnEarlyDraw(GameTime gameTime)
        {
        }

        private void DrawRenderStatsitics()
        {
            var renderStatistics = RenderStatistics.Instance;

            const int spacing = 5;
            var elementHeight = _debugSpriteFont.LineSpacing + spacing;
            var height = elementHeight * 4 + spacing;
            const int width = 180;

            var startPosition = new Vector2(GameInstance.ScreenBounds.Width - width, GameInstance.ScreenBounds.Height - height);

            GameInstance.SpriteBatch.Begin();
            GameInstance.ShapeRenderer.DrawRectangle((int)startPosition.X,
                (int)startPosition.Y,
                width,
                height,
                Color.DarkGreen);

            startPosition.X += spacing;
            startPosition.Y += spacing;
            GameInstance.SpriteBatch.DrawString(_debugSpriteFont,
                string.Format("Average DrawTime[ms]: {0:0.00}", renderStatistics.AverageDrawTime), startPosition, Color.White);
            startPosition.Y += elementHeight;
            GameInstance.SpriteBatch.DrawString(_debugSpriteFont, string.Format("Total Drawcalls: {0}", renderStatistics.DrawCalls),
                startPosition, Color.White);
            GameInstance.SpriteBatch.End();
        }

        public void OnClosing()
        {
        }

        #region overworld ui element handling:

        private List<OverworldUIElement> _uiElements = new List<OverworldUIElement>();

        public void AddUIElement(OverworldUIElement element)
        {
            lock (_uiElements)
                _uiElements.Add(element);
            element.Screen = this;
        }

        public void RemoveUIElement(OverworldUIElement element)
        {
            lock (_uiElements)
                _uiElements.Remove(element);
        }

        public bool HasBlockingElements
        {
            get { return _uiElements.Any(e => e.IsActive && e.IsBlocking); }
        }

        #endregion
    }
}
