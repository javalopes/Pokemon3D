using Pokemon3D.DataModel;
using Pokemon3D.DataModel.GameMode.Battle;
using Pokemon3D.DataModel.GameMode.Definitions;
using Pokemon3D.FileSystem;
using Pokemon3D.GameModes.Maps;
using Pokemon3D.GameModes.Pokemon;
using Pokemon3D.Rendering.Data;
using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.GameModes
{
    /// <summary>
    /// Contains methods and members that control a GameMode, a collection of maps, scripts and assets.
    /// </summary>
    partial class GameMode :  IDataModelContainer, IDisposable, GameModeDataProvider
    {
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
        public GameMode(GameModeInfo gameModeInfo, FileProvider fileLoader)
        {
            GameModeInfo = gameModeInfo;
            FileLoader = fileLoader;

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
                Path.Combine(GameModeInfo.DirectoryPath, PrimitivesFilePath),
                Path.Combine(GameModeInfo.DirectoryPath, NaturesFilePath),
                Path.Combine(GameModeInfo.DirectoryPath, TypesFilePath),
                Path.Combine(GameModeInfo.DirectoryPath, MoveFilesPath)
            }, OnLoadFinished);
        }

        private void OnLoadFinished(byte[][] data)
        {
            _primitiveModels = DataModel<PrimitiveModel[]>.FromByteArray(data[0]);
            _natureModels = DataModel<NatureModel[]>.FromByteArray(data[1]);
            _typeModels = DataModel<TypeModel[]>.FromByteArray(data[2]);
            _moveModels = DataModel<MoveModel[]>.FromByteArray(data[3]);
        }

        public GeometryData GetPrimitiveData(string primitiveName)
        {
            PrimitiveModel primitiveModel = _primitiveModels.SingleOrDefault(x => x.Id == primitiveName);
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
