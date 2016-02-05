using Pokemon3D.DataModel;
using Pokemon3D.DataModel.GameMode.Battle;
using Pokemon3D.DataModel.GameMode.Pokemon;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System;
using System.Linq;

namespace Pokemon3D.Editor.Core.Model
{
    public enum ResourceType
    {
        JsonFile,
        TextureFile,
        File,
        Model,
    }

    public abstract class ResourceModel
    {
        public string FilePath { get; set; }
        public string Name { get; set; }
        public string[] HierarchyPath { get; set; }

        public ResourceType ResourceType { get; set; }

        protected ResourceModel(string basePath, string filePath)
        {
            FilePath = filePath;
            var localPath = (Path.GetDirectoryName(filePath) ?? "").Replace(basePath.Trim(), "");
            HierarchyPath = localPath.Split(new[] { Path.DirectorySeparatorChar }, System.StringSplitOptions.RemoveEmptyEntries);
        }
    }

    public class TextureModel : ResourceModel
    {
        public TextureModel(string basePath, string filePath) : base(basePath, filePath)
        {
            ResourceType = ResourceType.TextureFile;
            Name = Path.GetFileName(filePath);
        }
    }

    public class ModelModel : ResourceModel
    {
        public ModelModel(string basePath, string[] filePartPaths) : base(basePath, filePartPaths.First())
        {
            ResourceType = ResourceType.Model;
            Name = Path.GetFileNameWithoutExtension(filePartPaths.First(f => f.EndsWith(".obj", StringComparison.OrdinalIgnoreCase)));
            HierarchyPath = HierarchyPath.Take(HierarchyPath.Length - 1).ToArray();
        }
    }

    public class GameModeModel
    {
        private const string FolderNameContent = "Content";
        private const string FolderNameTextures = "Textures";
        private const string FolderNameModels = "Models";

        private const string FolderNameData = "Data";
        private const string FolderNameMoves = "Moves";
        private const string FolderNamePokemon = "Pokemon";

        private const string FolderNameFragments = "Fragments";

        private const string FolderNameMaps = "Maps";

        private const string FolderNameScripts = "Scripts";

        private List<TextureModel> _textureModels;
        private List<ModelModel> _modelModels;
        private List<MoveModel> _moveModels;
        private List<PokemonModel> _pokemonModel;

        public GameModeModel()
        {
            _textureModels = new List<TextureModel>();
            TextureModels = _textureModels.AsReadOnly();

            _modelModels = new List<ModelModel>();
            ModelModels = _modelModels.AsReadOnly();

            _moveModels = new List<MoveModel>();
            MoveModels = _moveModels.AsReadOnly();

            _pokemonModel = new List<PokemonModel>();
            PokemonModels = _pokemonModel.AsReadOnly();
        }

        public static GameModeModel Create(string folderPath)
        {
            EnsureDefaultFoldersExists(folderPath);

            var gameMode = new GameModeModel();
            return gameMode;
        }

        public static GameModeModel Open(string folderPath)
        {
            EnsureDefaultFoldersExists(folderPath);

            var gameMode = new GameModeModel();

            ReadContentFolder(gameMode, folderPath);
            ReadDataFolder(gameMode, folderPath);

            return gameMode;
        }
        
        private static void ReadContentFolder(GameModeModel model, string folderPath)
        {
            var texturesPath = Path.Combine(folderPath, FolderNameContent, FolderNameTextures);
            FileSystem.GetFilesRecursive(texturesPath, file => model.AddTexture(new TextureModel(texturesPath, file)));

            var modelPath = Path.Combine(folderPath, FolderNameContent, FolderNameModels);
            FileSystem.GetFilesOfFolderRecursive(modelPath, files =>
            {
                if (files.Any(f => (Path.GetExtension(f) ?? "").Equals(".obj", StringComparison.OrdinalIgnoreCase)))
                {
                    model.AddModel(new ModelModel(modelPath, files));
                }
            });
        }

        private static void ReadDataFolder(GameModeModel gameMode, string folderPath)
        {
            FileSystem.GetFiles(Path.Combine(folderPath, FolderNameData, FolderNameMoves), f => gameMode.AddMove(DataModel<MoveModel>.FromFile(f)));
            FileSystem.GetFiles(Path.Combine(folderPath, FolderNameData, FolderNamePokemon), f => gameMode.AddPokemon(DataModel<PokemonModel>.FromFile(f)));
        }

        private static void EnsureDefaultFoldersExists(string folderPath)
        {
            FileSystem.EnsureFolderExists(Path.Combine(folderPath, FolderNameContent));
            FileSystem.EnsureFolderExists(Path.Combine(folderPath, FolderNameContent, FolderNameTextures));
            FileSystem.EnsureFolderExists(Path.Combine(folderPath, FolderNameContent, FolderNameModels));
            FileSystem.EnsureFolderExists(Path.Combine(folderPath, FolderNameData));
            FileSystem.EnsureFolderExists(Path.Combine(folderPath, FolderNameData, FolderNameMoves));
            FileSystem.EnsureFolderExists(Path.Combine(folderPath, FolderNameData, FolderNamePokemon));
            FileSystem.EnsureFolderExists(Path.Combine(folderPath, FolderNameFragments));
            FileSystem.EnsureFolderExists(Path.Combine(folderPath, FolderNameMaps));
            FileSystem.EnsureFolderExists(Path.Combine(folderPath, FolderNameScripts));
        }

        public ReadOnlyCollection<TextureModel> TextureModels { get; private set; }

        public ReadOnlyCollection<ModelModel> ModelModels { get; private set; }

        public ReadOnlyCollection<MoveModel> MoveModels { get; private set; }

        public ReadOnlyCollection<PokemonModel> PokemonModels { get; private set; }

        public void AddTexture(TextureModel textureModel)
        {
            _textureModels.Add(textureModel);
        }

        public void AddModel(ModelModel model)
        {
            _modelModels.Add(model);
        }

        public void AddMove(MoveModel model)
        {
            _moveModels.Add(model);
        }

        public void AddPokemon(PokemonModel model)
        {
            _pokemonModel.Add(model);
        }
    }
}