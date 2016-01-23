using Pokemon3D.DataModel;
using Pokemon3D.DataModel.GameMode.Battle;
using Pokemon3D.DataModel.GameMode.Definitions;
using Pokemon3D.GameModes.Maps;
using Pokemon3D.GameModes.Pokemon;
using Pokemon3D.Rendering.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.DataHandling;
using Pokemon3D.Common;

namespace Pokemon3D.GameModes
{
    /// <summary>
    /// Contains methods and members that control a GameMode, a collection of maps, scripts and assets.
    /// </summary>
    public partial class GameMode : GameContextObject, IDataModelContainer, IDisposable, GameModeDataProvider
    {
        private readonly Dictionary<string, AsyncTexture2D> _textureCache = new Dictionary<string, AsyncTexture2D>();
        private PrimitiveModel[] _primitiveModels;
        private NatureModel[] _natureModels;
        private TypeModel[] _typeModels;
        private MoveModel[] _moveModels;

        public FileProvider FileLoader { get; }
        public GameModeInfo GameModeInfo { get; }
        public MapManager MapManager { get; private set; }
        public MapFragmentManager MapFragmentManager { get; private set; }

        public PokemonFactory PokemonFactory { get; private set; }

        public bool IsValid { get; }

        /// <summary>
        /// Creates an instance of the <see cref="GameMode"/> class and loads the data model.
        /// </summary>
        internal GameMode(GameModeInfo gameModeInfo, GameContext gameContext, FileProvider fileLoader) : base(gameContext)
        {
            GameModeInfo = gameModeInfo;
            FileLoader = fileLoader;

            _textureCache = new Dictionary<string, AsyncTexture2D>();

            // only continue if the game mode config file loaded correctly.
            if (GameModeInfo.IsValid)
            {
                MapManager = new MapManager(this);
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
                TypesFilePath
            }, OnLoadFinished);
            FileLoader.GetFilesOfFolderAsync(MoveFilesPath, d => OnMovesLoaded(d, finished));
        }

        public AsyncTexture2D GetTexture(string filePath)
        {
            AsyncTexture2D existing;
            if (_textureCache.TryGetValue(filePath, out existing)) return existing;

            existing = new AsyncTexture2D();
            FileLoader.GetFileAsync(Path.Combine(TexturePath, filePath), d =>
            {
                GameContext.MainThreadDispatcher.Invoke(() =>
                {
                    using (var memoryStream = new MemoryStream(d.Data))
                    {
                        existing.SetTexture(Texture2D.FromStream(GameContext.GraphicsDevice, memoryStream));
                    }
                });
            });
            return existing;
        }
        
        private void OnMovesLoaded(DataLoadResult[] data, Action finished)
        {
            _moveModels = data.Select(d => DataModel<MoveModel>.FromByteArray(d.Data)).ToArray();
            finished();
        }

        private void OnLoadFinished(DataLoadResult[] data)
        {
            _primitiveModels = DataModel<PrimitiveModel[]>.FromByteArray(data[0].Data);
            _natureModels = DataModel<NatureModel[]>.FromByteArray(data[1].Data);
            _typeModels = DataModel<TypeModel[]>.FromByteArray(data[2].Data);
        }

        public GeometryData GetPrimitiveData(string primitiveName)
        {
            var primitiveModel = _primitiveModels.SingleOrDefault(x => x.Id == primitiveName);
            if (primitiveModel != null)
            {
                return new GeometryData
                {
                    Vertices = primitiveModel.Vertices.Select(v => new VertexPositionNormalTexture
                    {
                        Position = v.Position.GetVector3(),
                        TextureCoordinate = v.TexCoord.GetVector2(),
                        Normal = v.Normal.GetVector3()
                    }).ToArray(),
                    Indices = primitiveModel.Indices.Select(i => (ushort)i).ToArray()
                };
            }

            return null;
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
            return _typeModels.Single(n => n.Id == typeId);
        }

        public MoveModel GetMoveModel(string id)
        {
            return _moveModels.Single(m => m.Id == id);
        }

        public NatureModel[] GetNatures()
        {
            return _natureModels;
        }
    }
}
