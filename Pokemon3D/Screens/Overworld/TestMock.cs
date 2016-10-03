using Pokemon3D.DataModel.Pokemon;
using Pokemon3D.DataModel.Savegame;
using Pokemon3D.DataModel.Savegame.Inventory;
using Pokemon3D.DataModel.Savegame.Pokemon;
using Pokemon3D.GameModes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.Screens.Overworld
{
    static class TestMock
    {
        /// <summary>
        /// creates a temporary save until we get full load/save management done.
        /// </summary>
        public static SaveGame CreateTempSave()
        {
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

            return new SaveGame(dataModel);
        }
    }
}
