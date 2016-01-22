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

        private const string JSON_FILE_EXTENSION = ".json";
        private const string FILE_DATA_NATURES = "Natures";
        private const string FILE_DATA_TYPES = "Types";
        private const string FILE_DATA_PRIMITIVES = "Primitives";

        private const string PATH_MAPS = "Maps";
        private const string PATH_FRAGMENTS = "Fragments";
        private const string PATH_DATA = "Data";
        private const string PATH_DATA_POKEMON = "Pokemon";

        private const string PATH_DATA_MOVES = "Moves";

        /// <summary>
        /// The path to the texture base folder of this GameMode.
        /// </summary>
        public string TexturePath => Path.Combine(GameModeInfo.DirectoryPath, PATH_CONTENT, PATH_CONTENT_TEXTURES);

        /// <summary>
        /// The absolute path to the texture base folder of this GameMode.
        /// </summary>
        public string DataPath => Path.Combine(GameModeInfo.DirectoryPath, PATH_DATA);

        /// <summary>
        /// The absolute path to the pokemon data base folder of this GameMode.
        /// </summary>
        public string PokemonDataPath => Path.Combine(GameModeInfo.DirectoryPath, PATH_DATA, PATH_DATA_POKEMON);

        /// <summary>
        /// The absolute path to the pokemon data base folder of this GameMode.
        /// </summary>
        public string i18nPath => Path.Combine(GameModeInfo.DirectoryPath, PATH_DATA, PATH_DATA_i18n);

        /// <summary>
        /// The absolute path to the directory that contains the move files.
        /// </summary>
        public string MoveFilesPath => Path.Combine(GameModeInfo.DirectoryPath, PATH_DATA, PATH_DATA_MOVES);

        /// <summary>
        /// The file path to the file containing the nature data.
        /// </summary>
        public string NaturesFilePath => Path.Combine(GameModeInfo.DirectoryPath, PATH_DATA, FILE_DATA_NATURES + JSON_FILE_EXTENSION);

        /// <summary>
        /// The file path to the file containing the Pokémon type data.
        /// </summary>
        public string TypesFilePath => Path.Combine(GameModeInfo.DirectoryPath, PATH_DATA, FILE_DATA_TYPES + JSON_FILE_EXTENSION);

        /// <summary>
        /// The file path to the file containing the primitive data.
        /// </summary>
        public string PrimitivesFilePath => Path.Combine(GameModeInfo.DirectoryPath, PATH_DATA, FILE_DATA_PRIMITIVES + JSON_FILE_EXTENSION);

        /// <summary>
        /// Returns the path to a map file, relative to the GameMode's root folder.
        /// </summary>
        public string GetMapFilePath(string mapId)
        {
            return Path.Combine(GameModeInfo.DirectoryPath, PATH_MAPS, mapId + JSON_FILE_EXTENSION);
        }

        /// <summary>
        /// Returns the path to a map fragment file, relative to the GameMode's root folder.
        /// </summary>
        public string GetMapFragmentFilePath(string mapFragmentId)
        {
            return Path.Combine(GameModeInfo.DirectoryPath, PATH_FRAGMENTS, mapFragmentId + JSON_FILE_EXTENSION);
        }

        /// <summary>
        /// Returns the path to a pokemon data file, relative to the GameMode's root folder.
        /// </summary>
        public string GetPokemonFilePath(string pokemonId)
        {
            return Path.Combine(PATH_DATA, PATH_DATA_POKEMON, pokemonId + JSON_FILE_EXTENSION);
        }

        public string GetPokemonTexturesContentPath()
        {
            return Path.Combine(GameModeInfo.DirectioryName, PATH_CONTENT, PATH_CONTENT_TEXTURES, "Pokemon");
        }
    }
}
