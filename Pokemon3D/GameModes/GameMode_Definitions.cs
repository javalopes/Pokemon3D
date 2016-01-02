using System.IO;

namespace Pokemon3D.GameModes
{
    /// <summary>
    /// Contains definitions to reduce clutter in the main class file.
    /// </summary>
    partial class GameMode
    {
        private const string PATH_CONTENT = "Content";
        private const string PATH_CONTENT_TEXTURES = "Textures";
        private const string PATH_CONTENT_MODELS = "Models";
        private const string PATH_DATA_i18n = "i18n";

        public const string FILE_DATA_PRIMITIVES = "Primitives.json";

        /// <summary>
        /// The path to the texture base folder of this GameMode.
        /// </summary>
        public string TexturePath => Path.Combine(GameModeInfo.DirectoryPath, PATH_CONTENT, PATH_CONTENT_TEXTURES);

        /// <summary>
        /// The path to the texture base folder of this GameMode.
        /// </summary>
        public string DataPath => Path.Combine(GameModeInfo.DirectoryPath, PATH_DATA);
        
        public string PokemonDataPath => Path.Combine(DataPath, PATH_DATA_POKEMON);

        public string i18nPath => Path.Combine(DataPath, PATH_DATA_i18n);

        private const string JSON_FILE_EXTENSION = ".json";
        private const string FILE_DATA_NATURES = "Natures";
        private const string FILE_DATA_TYPES = "Types";

        private const string PATH_MAPS = "Maps";
        private const string PATH_FRAGMENTS = "Fragments";
        private const string PATH_DATA = "Data";
        private const string PATH_DATA_POKEMON = "Pokemon";

        /// <summary>
        /// Returns the path to a map file, relative to the GameMode's root folder.
        /// </summary>
        public string GetMapFilePath(string mapId)
        {
            return Path.Combine(PATH_MAPS, mapId + JSON_FILE_EXTENSION);
        }

        /// <summary>
        /// Returns the path to a map fragment file, relative to the GameMode's root folder.
        /// </summary>
        public string GetMapFragmentFilePath(string mapFragmentId)
        {
            return Path.Combine(PATH_FRAGMENTS, mapFragmentId + JSON_FILE_EXTENSION);
        }

        /// <summary>
        /// Returns the path to a pokemon data file, relative to the GameMode's root folder.
        /// </summary>
        public string GetPokemonFilePath(string pokemonId)
        {
            return Path.Combine(PATH_DATA, PATH_DATA_POKEMON, pokemonId + JSON_FILE_EXTENSION);
        }

        /// <summary>
        /// The file path to the file containing the nature data.
        /// </summary>
        public string NaturesFilePath => Path.Combine(PATH_DATA, FILE_DATA_NATURES + JSON_FILE_EXTENSION);

        /// <summary>
        /// The file path to the file containing the Pokémon type data.
        /// </summary>
        public string TypesFilePath => Path.Combine(PATH_DATA, FILE_DATA_TYPES + JSON_FILE_EXTENSION);
    }
}
