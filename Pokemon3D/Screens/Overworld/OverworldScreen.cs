using Microsoft.Xna.Framework;
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
using Pokemon3D.Rendering;
using Pokemon3D.Common.Input;
using Pokemon3D.Collisions;
using Pokemon3D.Common.Shapes;

namespace Pokemon3D.Screens.Overworld
{
    class OverworldScreen : Screen, WorldContainer
    {
        public World ActiveWorld { get; private set; }

        private SpriteFont _debugSpriteFont;
        private bool _showRenderStatistics;

        private bool _isLoaded;

        public void OnOpening(object enterInformation)
        {
            if (!_isLoaded)
            {
                ActiveWorld = enterInformation as World;
                if (ActiveWorld == null) throw new InvalidOperationException("Did not receive loaded data.");

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
            GameInstance.LoadedSave.Load(GameInstance.GetService<GameModeManager>().ActiveGameMode);
        }

        public void OnUpdate(GameTime gameTime)
        {
            var inputSystem = GameInstance.GetService<InputSystem>();
            var screenManager = GameInstance.GetService<ScreenManager>();
            var sceneRenderer = GameInstance.GetService<SceneRenderer>();

            ActiveWorld.Update(gameTime);

            lock (_uiElements)
                _uiElements.ForEach(e => { if (e.IsActive) e.Update(gameTime); });

            if (inputSystem.Keyboard.IsKeyDown(Keys.Escape))
            {
                screenManager.SetScreen(typeof(MainMenuScreen), typeof(BlendTransition));
            }

            if (inputSystem.Keyboard.IsKeyDownOnce(Keys.L))
            {
                sceneRenderer.EnablePostProcessing = !sceneRenderer.EnablePostProcessing;
            }

            if (inputSystem.Keyboard.IsKeyDownOnce(Keys.F12))
            {
                _showRenderStatistics = !_showRenderStatistics;
            }

            if (inputSystem.Keyboard.IsKeyDownOnce(Keys.X) || inputSystem.GamePad.IsButtonDownOnce(Buttons.X))
            {
                screenManager.SetScreen(typeof(GameMenu.GameMenuScreen));
            }
        }

        public void OnLateDraw(GameTime gameTime)
        {
            GameInstance.GetService<CollisionManager>().Draw(ActiveWorld.Player.Camera);
            if (_showRenderStatistics) DrawRenderStatsitics();

            bool anyActive;
            lock (_uiElements)
            {
                anyActive = _uiElements.Count > 0 && _uiElements.Any(e => e.IsActive);
            }

            if (anyActive)
            {
                var spriteBatch = GameInstance.GetService<SpriteBatch>();
                spriteBatch.Begin();

                lock (_uiElements)
                {
                    _uiElements.ForEach(e => { if (e.IsActive) e.Draw(gameTime); });
                }

                spriteBatch.End();
            }

#if DEBUG_RENDERING
            GameInstance.SceneRenderer.LateDebugDraw3D();
#endif
        }

        public void OnEarlyDraw(GameTime gameTime)
        {
        }

        private void DrawRenderStatsitics()
        {
            var renderStatistics = RenderStatistics.Instance;
            var spriteBatch = GameInstance.GetService<SpriteBatch>();

            const int spacing = 5;
            var elementHeight = _debugSpriteFont.LineSpacing + spacing;
            var height = elementHeight * 3 + spacing;
            const int width = 180;

            var startPosition = new Vector2(0,GameInstance.ScreenBounds.Height-height);

            spriteBatch.Begin();
            GameInstance.GetService<ShapeRenderer>().DrawRectangle((int)startPosition.X,
                (int)startPosition.Y,
                width,
                height,
                Color.DarkGreen);

            startPosition.X += spacing;
            startPosition.Y += spacing;
            spriteBatch.DrawString(_debugSpriteFont, $"Average DrawTime[ms]: {renderStatistics.AverageDrawTime:0.00}", startPosition, Color.White);
            startPosition.Y += elementHeight;
            spriteBatch.DrawString(_debugSpriteFont, $"Total Drawcalls: {renderStatistics.DrawCalls}", startPosition, Color.White);
            startPosition.Y += elementHeight;
            spriteBatch.DrawString(_debugSpriteFont, $"Entity Count: {ActiveWorld.EntitySystem.EntityCount}", startPosition, Color.White);
            spriteBatch.End();
        }

        public void OnClosing()
        {
        }

        #region overworld ui element handling:

        private readonly List<OverworldUIElement> _uiElements = new List<OverworldUIElement>();

        public void AddUiElement(OverworldUIElement element)
        {
            lock (_uiElements) _uiElements.Add(element);
            element.Screen = this;
        }

        public void RemoveUiElement(OverworldUIElement element)
        {
            lock (_uiElements) _uiElements.Remove(element);
        }

        public bool HasBlockingElements
        {
            get
            {
                lock (_uiElements)
                {
                    return _uiElements.Any(e => e.IsActive && e.IsBlocking);
                }
            }
        }

        #endregion
    }
}
