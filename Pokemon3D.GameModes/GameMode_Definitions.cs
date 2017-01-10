using System.IO;

namespace Pokemon3D.GameModes
{
    /// <summary>
    /// Contains definitions to reduce clutter in the main class file.
    /// </summary>
    partial class GameMode
    {
        private const string PathContent = "Content";
        private const string PathContentTextures = "Textures";
        private const string PathContentModels = "Models";
        private const string PathDataI18N = "i18n";

        private const string JsonFileExtension = ".json";
        private const string JsFileExtension = ".js";
        private const string FileDataNatures = "Natures";
        private const string FileDataTypes = "Types";
        private const string FileDataPrimitives = "Primitives";
        private const string FileDataPokedexes = "Pokedexes";

        private const string PathMaps = "Maps";
        private const string PathScripts = "Scripts";
        private const string PathFragments = "Fragments";
        private const string PathData = "Data";
        private const string PathDataPokemon = "Pokemon";

        private const string PathDataMoves = "Moves";
        private const string PathDataItems = "Items";
        private const string PathDataAbilities = "Abilities";

        /// <summary>
        /// The path to the texture base folder of this GameMode.
        /// </summary>
        public string TexturePath => Path.Combine(GameModeInfo.DirectoryPath, PathContent, PathContentTextures);

        /// <summary>
        /// The path to the 3d models folder of this GameMode.
        /// </summary>
        public string ModelPath => Path.Combine(GameModeInfo.DirectoryPath, PathContent, PathContentModels);

        /// <summary>
        /// The absolute path to the texture base folder of this GameMode.
        /// </summary>
        public string DataPath => Path.Combine(GameModeInfo.DirectoryPath, PathData);

        /// <summary>
        /// The absolute path to the pokemon data base folder of this GameMode.
        /// </summary>
        public string PokemonDataPath => Path.Combine(GameModeInfo.DirectoryPath, PathData, PathDataPokemon);

        /// <summary>
        /// The absolute path to the pokemon data base folder of this GameMode.
        /// </summary>
        public string I18NPath => Path.Combine(GameModeInfo.DirectoryPath, PathData, PathDataI18N);

        /// <summary>
        /// The absolute path to the directory that contains the move files.
        /// </summary>
        public string MoveFilesPath => Path.Combine(GameModeInfo.DirectoryPath, PathData, PathDataMoves);

        /// <summary>
        /// The absolute path to the directory that contains the item files.
        /// </summary>
        public string ItemFilesPath => Path.Combine(GameModeInfo.DirectoryPath, PathData, PathDataItems);

        /// <summary>
        /// The absolute path to the directory that contains the ability files.
        /// </summary>
        public string AbilityFilesPath => Path.Combine(GameModeInfo.DirectoryPath, PathData, PathDataAbilities);

        /// <summary>
        /// The file path to the file containing the nature data.
        /// </summary>
        public string NaturesFilePath => Path.Combine(GameModeInfo.DirectoryPath, PathData, FileDataNatures + JsonFileExtension);

        /// <summary>
        /// The file path to the file containing the Pokémon type data.
        /// </summary>
        public string TypesFilePath => Path.Combine(GameModeInfo.DirectoryPath, PathData, FileDataTypes + JsonFileExtension);

        /// <summary>
        /// The file path to the file containing the Pokédex definition data.
        /// </summary>
        public string PokedexesFilePath => Path.Combine(GameModeInfo.DirectoryPath, PathData, FileDataPokedexes + JsonFileExtension);

        /// <summary>
        /// The file path to the file containing the primitive data.
        /// </summary>
        public string PrimitivesFilePath => Path.Combine(GameModeInfo.DirectoryPath, PathData, FileDataPrimitives + JsonFileExtension);

        /// <summary>
        /// Returns the path to a map file, relative to the GameMode's root folder.
        /// </summary>
        public string GetMapFilePath(string mapId)
        {
            return Path.Combine(GameModeInfo.DirectoryPath, PathMaps, mapId + JsonFileExtension);
        }

        /// <summary>
        /// Returns the path to a script file, relative to the GameMode's root folder.
        /// </summary>
        public string GetScriptFilePath(string scriptFileName)
        {
            // if the input file path has no file extension, add the default ".js" one.
            if (!Path.HasExtension(scriptFileName))
                scriptFileName += JsFileExtension;

            return Path.Combine(GameModeInfo.DirectoryPath, PathScripts, scriptFileName);
        }

        /// <summary>
        /// Returns the path to a map fragment file, relative to the GameMode's root folder.
        /// </summary>
        public string GetMapFragmentFilePath(string mapFragmentId)
        {
            return Path.Combine(GameModeInfo.DirectoryPath, PathFragments, mapFragmentId + JsonFileExtension);
        }

        /// <summary>
        /// Returns the absolute path to a pokemon data file.
        /// </summary>
        public string GetPokemonFilePath(string pokemonId)
        {
            return Path.Combine(GameModeInfo.DirectoryPath, PathData, PathDataPokemon, pokemonId + JsonFileExtension);
        }

        public string GetPokemonTexturesContentPath()
        {
            return Path.Combine(GameModeInfo.DirectioryName, PathContent, PathContentTextures, "Pokemon");
        }
    }
}
