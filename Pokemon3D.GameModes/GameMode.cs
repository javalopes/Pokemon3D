﻿using Pokemon3D.DataModel;
using Pokemon3D.DataModel.GameMode.Battle;
using Pokemon3D.DataModel.GameMode.Definitions;
using Pokemon3D.Rendering.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.DataHandling;
using Pokemon3D.Common;
using Mesh = Pokemon3D.Rendering.Data.Mesh;
using Pokemon3D.DataModel.GameMode.Pokemon;
using Pokemon3D.DataModel.GameMode.Items;
using Pokemon3D.DataModel.GameMode.Map;
using Pokemon3D.GameModes.Maps;
using Pokemon3D.GameModes.Monsters;

namespace Pokemon3D.GameModes
{
    public partial class GameMode : GameContextObject, IDataModelContainer, IDisposable
    {
        private readonly Dictionary<string, ModelMesh[]> _meshCache = new Dictionary<string, ModelMesh[]>();
        private readonly Dictionary<string, Mesh> _meshPrimitivesByName = new Dictionary<string, Mesh>();
        private readonly Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();

        private PrimitiveModel[] _primitiveModels;
        private NatureModel[] _natureModels;
        private TypeModel[] _typeModels;
        private MoveModel[] _moveModels;
        private ItemModel[] _itemModels;
        private PokedexModel[] _pokedexModels;
        private AbilityModel[] _abilityModels;

        public IFileProvider IFileLoader { get; }
        public GameModeInfo GameModeInfo { get; }
        public MapFragmentManager MapFragmentManager { get; private set; }
        public PokemonFactory PokemonFactory { get; private set; }

        public bool IsValid { get; }

        public SaveGame SaveGame { get; private set; }

        public GraphicsDevice GraphicsDevice => GameContext.GetService<GraphicsDevice>();

        /// <summary>
        /// Creates an instance of the <see cref="GameMode"/> class and loads the data model.
        /// </summary>
        internal GameMode(GameModeInfo gameModeInfo, IGameContext iGameContext, IFileProvider iFileLoader) : base(iGameContext)
        {
            GameModeInfo = gameModeInfo;
            IFileLoader = iFileLoader;

            // only continue if the game mode config file loaded correctly.
            if (GameModeInfo.IsValid)
            {
                MapFragmentManager = new MapFragmentManager(this);
                PokemonFactory = new PokemonFactory(this);
            }

            IsValid = true;
        }
        
        private static long ComputeCrc(byte[] val)
        {
            long crc = 0;
            foreach (var c in val)
            {
                var q = (crc ^ c) & 0x0f;
                crc = (crc >> 4) ^ (q * 0x1081);
                q = (crc ^ (c >> 4)) & 0xf;
                crc = (crc >> 4) ^ (q * 0x1081);
            }
            return crc;
        }

        public long CalculateChecksum()
        {
            var contentFolder = Path.Combine(GameModeInfo.DirectoryPath, PathContent);
            var files = new List<string>();
            AddFilesRecursive(contentFolder, files);

            var checkSum = 0L;
            var encoding = new UnicodeEncoding();

            foreach (var file in files)
            {
                checkSum += ComputeCrc(encoding.GetBytes(file));
                checkSum += ComputeCrc(File.ReadAllBytes(file));
            }

            return checkSum;
        }

        private void AddFilesRecursive(string parentFolder, List<string> files)
        {
            foreach (var filePath in Directory.GetFiles(parentFolder))
            {
                files.Add(filePath);
            }

            foreach (var directoryPath in Directory.GetDirectories(parentFolder))
            {
                AddFilesRecursive(directoryPath, files);
            }
        }

        public void LoadSaveGame(SaveGame saveGame)
        {
            SaveGame = saveGame;
            saveGame.Load(this);
        }

        public void Preload()
        {
            var data = IFileLoader.GetFiles(new[]
            {
                PrimitivesFilePath,
                NaturesFilePath,
                TypesFilePath,
                PokedexesFilePath
            });

            _primitiveModels = DataModel<PrimitiveModel[]>.FromByteArray(data[0].Data);
            foreach (var primitiveModel in _primitiveModels)
            {
                var geometryData = new GeometryData
                {
                    Vertices = primitiveModel.Vertices.Select(v => new VertexPositionNormalTexture
                    {
                        Position = v.Position.GetVector3(),
                        TextureCoordinate = v.TexCoord.GetVector2(),
                        Normal = v.Normal.GetVector3()
                    }).ToArray(),
                    Indices = primitiveModel.Indices.Select(i => (ushort)i).ToArray()
                };

                Mesh mesh = null;
                GameContext.GetService<JobSystem>().EnsureExecutedInMainThread(() => mesh = new Mesh(GraphicsDevice, geometryData));
                _meshPrimitivesByName.Add(primitiveModel.Id, mesh);
            }

            _natureModels = DataModel<NatureModel[]>.FromByteArray(data[1].Data);
            _typeModels = DataModel<TypeModel[]>.FromByteArray(data[2].Data);
            _pokedexModels = DataModel<PokedexModel[]>.FromByteArray(data[3].Data);

            var movesFilePaths = IFileLoader.GetFilesOfFolder(MoveFilesPath);
            _moveModels = movesFilePaths.Select(d => DataModel<MoveModel>.FromByteArray(d.Data)).ToArray();

            var itemsFiles = IFileLoader.GetFilesOfFolder(ItemFilesPath);
            _itemModels = itemsFiles.Select(d => DataModel<ItemModel>.FromByteArray(d.Data)).ToArray();

            var abilityFiles = IFileLoader.GetFilesOfFolder(AbilityFilesPath);
            _abilityModels = abilityFiles.Select(d => DataModel<AbilityModel>.FromByteArray(d.Data)).ToArray();
        }

        public Texture2D GetTexture(string filePath)
        {
            return GetTextureFromRawFolder(Path.Combine(TexturePath, filePath));
        }

        public Texture2D GetTextureFromRawFolder(string filePath)
        {
            Texture2D existing;
            if (_textureCache.TryGetValue(filePath, out existing)) return existing;

            var data = IFileLoader.GetFile(Path.Combine(TexturePath, filePath));

            Texture2D texture = null;
            GameContext.GetService<JobSystem>().EnsureExecutedInMainThread(() =>
            {
                using (var memoryStream = new MemoryStream(data.Data))
                {
                    texture = Texture2D.FromStream(GraphicsDevice, memoryStream);
                }
            });

            _textureCache.Add(filePath, texture);
            return texture;
        }

        public MapModel LoadMap(string dataPath, bool forceReloadCached = false)
        {
            var data = IFileLoader.GetFile(GetMapFilePath(dataPath), forceReloadCached);
            return DataModel<MapModel>.FromByteArray(data.Data);
        }

        public ModelMesh[] GetModel(string filePath)
        {
            ModelMesh[] meshes;
            if (_meshCache.TryGetValue(filePath, out meshes)) return meshes;

            var absolutePath = Path.Combine(ModelPath, filePath);
            var meshsArray = ModelMesh.LoadFromFile(this, absolutePath);
            _meshCache.Add(filePath, meshsArray);
            return meshsArray;
        }

        public Mesh GetPrimitiveMesh(string primitiveName)
        {
            Mesh mesh;
            return _meshPrimitivesByName.TryGetValue(primitiveName, out mesh) ? mesh : null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }

            foreach (var meshCache in _meshCache)
            {
                foreach (var modelMesh in meshCache.Value)
                {
                    modelMesh.Dispose();
                }
            }
            _meshCache.Clear();

            foreach (var texture in _textureCache)
            {
                texture.Value.Dispose();
            }
            _textureCache.Clear();
        }

        public void Dispose()
        {
            Dispose(true);
        }
        
        ~GameMode()
        {
            Dispose(false);
        }

        public NatureModel GetNatureModel(string natureId)
        {
            return _natureModels.Single(n => n.Id == natureId);
        }

        public TypeModel GetTypeModel(string typeId)
        {
            return typeId == null ? null : _typeModels.Single(n => n.Id == typeId);
        }

        public MoveModel GetMoveModel(string id)
        {
            return _moveModels.Single(m => m.Id == id);
        }

        public PokedexModel GetPokedexModel(string id)
        {
            return _pokedexModels.Single(m => m.Id == id);
        }

        public ItemModel GetItemModel(string id)
        {
            return _itemModels.Single(m => m.Id == id);
        }

        public AbilityModel GetAbilityModel(string id)
        {
            return _abilityModels.Single(m => m.Id == id);
        }

        public NatureModel[] GetNatures()
        {
            return _natureModels;
        }

        public void EnsureExecutedInMainThread(Action action)
        {
            GameContext.GetService<JobSystem>().EnsureExecutedInMainThread(action);
        }
    }
}