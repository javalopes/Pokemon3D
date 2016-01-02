using Pokemon3D.DataModel.Json.GameMode.Pokemon;
using Pokemon3D.DataModel.Json.Savegame.Pokemon;
using Pokemon3D.GameModes;
using Pokemon3D.GameModes.Pokemon;

namespace Pokemon3D.FileSystem
{
    /// <summary>
    /// A request to read data from a Pokémon data file and create a <see cref="Pokemon"/> from that and additional information.
    /// </summary>
    class PokemonDataRequest : DataModelRequest<PokemonModel>
    {
        /// <summary>
        /// The <see cref="Pokemon"/> instance resulting from the data model and the additional information provided.
        /// </summary>
        public Pokemon ResultPokemon { get; set; }
        
        public PokemonSaveModel SaveModel { get; set; }

        public int StartLevel { get; private set; }
        
        /// <summary>
        /// Creates a data request from a path to a data model and the starting level of the Pokémon.
        /// </summary>
        public PokemonDataRequest(GameMode gameMode, string dataPath, int level)
            : base(gameMode, dataPath)
        {
            StartLevel = level;
        }

        /// <summary>
        /// Creates a data request from an already existing save model.
        /// </summary>
        public PokemonDataRequest(GameMode gameMode, PokemonSaveModel saveModel)
            : base(gameMode, gameMode.GetPokemonFilePath(saveModel.Id))
        {
            SaveModel = saveModel;
        }
    }
}
