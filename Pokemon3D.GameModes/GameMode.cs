using Pokemon3D.DataModel;
using Pokemon3D.DataModel.GameMode.Battle;
using Pokemon3D.DataModel.GameMode.Definitions;
using Pokemon3D.Entities.Maps;
using Pokemon3D.Entities.Pokemon;
using Pokemon3D.Rendering.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.DataHandling;
using Pokemon3D.Common;
using Mesh = Pokemon3D.Rendering.Data.Mesh;
using System.Windows.Threading;
using Pokemon3D.DataModel.GameMode.Pokemon;
using Pokemon3D.DataModel.GameMode.Items;
using Pokemon3D.DataModel.GameMode.Map;

namespace Pokemon3D.Entities
{
    /// <summary>
    /// Contains methods and members that control a GameMode, a collection of maps, scripts and assets.
    /// </summary>
    public partial class GameMode : GameContextObject, IDataModelContainer, IDisposable, ModelMeshContext
    {
        private readonly Dictionary<string, Rendering.Data.ModelMesh[]> _meshCache = new Dictionary<string, Rendering.Data.ModelMesh[]>();
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

        public Dispatcher MainThreadDispatcher
        {
            get { return GameContext.MainThreadDispatcher; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return GameContext.GraphicsDevice; }
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

        public void PreloadAsync(Action finished)
        {
            FileLoader.GetFilesAsync(new[]
            {
                PrimitivesFilePath,
                NaturesFilePath,
                TypesFilePath,
                PokedexesFilePath
            }, a => OnLoadFinished(a,finished));
        }

        private void OnLoadFinished(DataLoadResult[] data, Action finished)
        {
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
                var mesh = new Mesh(GameContext.GraphicsDevice, geometryData);
                _meshPrimitivesByName.Add(primitiveModel.Id, mesh);
            }

            _natureModels = DataModel<NatureModel[]>.FromByteArray(data[1].Data);
            _typeModels = DataModel<TypeModel[]>.FromByteArray(data[2].Data);
            _pokedexModels = DataModel<PokedexModel[]>.FromByteArray(data[3].Data);

            FileLoader.GetFilesOfFolderAsync(MoveFilesPath, d => OnMovesLoaded(d, finished));
        }

        private void OnMovesLoaded(DataLoadResult[] data, Action finished)
        {
            _moveModels = data.Select(d => DataModel<MoveModel>.FromByteArray(d.Data)).ToArray();
            FileLoader.GetFilesOfFolderAsync(ItemFilesPath, d => OnItemsLoaded(d, finished));
        }

        private void OnItemsLoaded(DataLoadResult[] data, Action finished)
        {
            _itemModels = data.Select(d => DataModel<ItemModel>.FromByteArray(d.Data)).ToArray();
            finished();
        }
        
        public AsyncTexture2D GetTextureAsync(string filePath)
        {
            var fullPath = Path.Combine(TexturePath, filePath);

            Texture2D existing;
            if (_textureCache.TryGetValue(fullPath, out existing)) return new AsyncTexture2D(existing);

            var newTextureLoadRequest = new AsyncTexture2D();
            FileLoader.GetFileAsync(Path.Combine(TexturePath, fullPath), d =>
            {
                var texture = GetTextureFromByteArrayDispatched(d.Data);
                _textureCache.Add(fullPath, texture);
                newTextureLoadRequest.SetTexture(texture);
            });
            return newTextureLoadRequest;
        }

        public Texture2D GetTexture(string filePath)
        {
            return GetTextureFromRawFolder(Path.Combine(TexturePath, filePath));
        }

        public Texture2D GetTextureFromRawFolder(string filePath)
        {
            Texture2D existing;
            if (_textureCache.TryGetValue(filePath, out existing)) return existing;

            existing = GetTextureFromByteArrayDispatched(FileLoader.GetFile(Path.Combine(TexturePath, filePath)));
            _textureCache.Add(filePath, existing);
            return existing;
        }

        private Texture2D GetTextureFromByteArrayDispatched(byte[] data)
        {
            Texture2D texture = null;
            using (var memoryStream = new MemoryStream(data))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                if (GameContext.MainThreadDispatcher.CheckAccess())
                {
                    texture = Texture2D.FromStream(GameContext.GraphicsDevice, memoryStream);
                }
                else
                {
                    GameContext.MainThreadDispatcher.Invoke(() =>
                    {
                        texture = Texture2D.FromStream(GameContext.GraphicsDevice, memoryStream);
                    });
                }
            }
            return texture;
        }

        public void LoadMapAsync(string dataPath, Action<MapModel> mapModelLoaded)
        {
            FileLoader.GetFileAsync(GetMapFilePath(dataPath), a =>
            {
                mapModelLoaded(DataModel<MapModel>.FromByteArray(a.Data));
            });
        }

        public void GetModelAsync(string filePath, Action<Rendering.Data.ModelMesh[]> modelLoaded)
        {
            throw new NotImplementedException("Currently disabled");
        }

        public Rendering.Data.ModelMesh[] GetModel(string filePath, Dispatcher mainThreadDispatcher = null)
        {
            Rendering.Data.ModelMesh[] meshes;
            if (_meshCache.TryGetValue(filePath, out meshes)) return meshes;

            var absolutePath = Path.Combine(ModelPath, filePath);
            var meshsArray = Rendering.Data.ModelMesh.LoadFromFile(this, absolutePath);
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
    }
}
