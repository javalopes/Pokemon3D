using Pokemon3D.DataModel;
using Pokemon3D.DataModel.GameMode.Battle;
using Pokemon3D.DataModel.GameMode.Definitions;
using Pokemon3D.Rendering.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// <summary>
    /// Contains methods and members that control a GameMode, a collection of maps, scripts and assets.
    /// </summary>
    public partial class GameMode : GameContextObject, IDataModelContainer, IDisposable
    {
        private readonly Dictionary<string, ModelMesh[]> _meshCache = new Dictionary<string, ModelMesh[]>();
        private readonly Dictionary<string, Mesh> _meshPrimitivesByName = new Dictionary<string, Mesh>(); 
        private readonly Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();
        private readonly Dictionary<string, MapModel> _mapModelsCache = new Dictionary<string, MapModel>(); 

        private PrimitiveModel[] _primitiveModels;
        private NatureModel[] _natureModels;
        private TypeModel[] _typeModels;
        private MoveModel[] _moveModels;
        private ItemModel[] _itemModels;
        private PokedexModel[] _pokedexModels;

        public FileProvider FileLoader { get; }
        public GameModeInfo GameModeInfo { get; }
        public MapFragmentManager MapFragmentManager { get; private set; }
        public PokemonFactory PokemonFactory { get; private set; }

        public bool IsValid { get; }

        public GraphicsDevice GraphicsDevice
        {
            get { return GameContext.GetService<GraphicsDevice>(); }
        }

        /// <summary>
        /// Creates an instance of the <see cref="GameMode"/> class and loads the data model.
        /// </summary>
        internal GameMode(GameModeInfo gameModeInfo, GameContext gameContext, FileProvider fileLoader) : base(gameContext)
        {
            GameModeInfo = gameModeInfo;
            FileLoader = fileLoader;

            // only continue if the game mode config file loaded correctly.
            if (GameModeInfo.IsValid)
            {
                MapFragmentManager = new MapFragmentManager(this);
                PokemonFactory = new PokemonFactory(this);
            }

            IsValid = true;
        }

        public void Preload()
        {
            var data = FileLoader.GetFiles(new[]
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
                GameContext.EnsureExecutedInMainThread(() => mesh = new Mesh(GraphicsDevice, geometryData));
                _meshPrimitivesByName.Add(primitiveModel.Id, mesh);
            }

            _natureModels = DataModel<NatureModel[]>.FromByteArray(data[1].Data);
            _typeModels = DataModel<TypeModel[]>.FromByteArray(data[2].Data);
            _pokedexModels = DataModel<PokedexModel[]>.FromByteArray(data[3].Data);

            var movesFilePaths = FileLoader.GetFilesOfFolder(MoveFilesPath);
            _moveModels = movesFilePaths.Select(d => DataModel<MoveModel>.FromByteArray(d.Data)).ToArray();
            
            var itemsFiles = FileLoader.GetFilesOfFolder(ItemFilesPath);
            _itemModels = itemsFiles.Select(d => DataModel<ItemModel>.FromByteArray(d.Data)).ToArray();
        }
                
        public Texture2D GetTexture(string filePath)
        {
            return GetTextureFromRawFolder(Path.Combine(TexturePath, filePath));
        }

        public Texture2D GetTextureFromRawFolder(string filePath)
        {
            Texture2D existing;
            if (_textureCache.TryGetValue(filePath, out existing)) return existing;
            
            var data = FileLoader.GetFile(Path.Combine(TexturePath, filePath), false);

            Texture2D texture = null;
            using (var memoryStream = new MemoryStream(data.Data))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                GameContext.EnsureExecutedInMainThread(() =>
                {
                    texture = Texture2D.FromStream(GraphicsDevice, memoryStream);
                });
            }
            
            _textureCache.Add(filePath, texture);
            return texture;
        }

        public MapModel LoadMap(string dataPath, bool forceReloadCached = false)
        {
            var data = FileLoader.GetFile(GetMapFilePath(dataPath), forceReloadCached);
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

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                // MapManager.Dispose();
                // PrimitiveManager.Dispose();
            }

            // todo: free unmanaged resources.
        }

        /// <summary>
        /// Frees all resources consumed by this GameMode.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Add, if this class has unmanaged resources
        //~GameMode()
        //{
        //    Dispose(false);
        //}

        #endregion

        public NatureModel GetNatureModel(string natureId)
        {
            return _natureModels.Single(n => n.Id == natureId);
        }

        public TypeModel GetTypeModel(string typeId)
        {
            if (typeId == null)
                return null;

            return _typeModels.Single(n => n.Id == typeId);
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

        public NatureModel[] GetNatures()
        {
            return _natureModels;
        }

        public void EnsureExecutedInMainThread(Action action)
        {
            GameContext.EnsureExecutedInMainThread(action);
        }
    }
}
