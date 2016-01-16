using Pokemon3D.Common.DataHandling;
using Pokemon3D.DataModel;
using Pokemon3D.DataModel.GameMode.Battle;
using Pokemon3D.DataModel.GameMode.Definitions;
using Pokemon3D.FileSystem;
using Pokemon3D.FileSystem.Requests;
using Pokemon3D.GameModes.Maps;
using Pokemon3D.GameModes.Pokemon;
using Pokemon3D.GameModes.Resources;
using Pokemon3D.Rendering.Data;
using System;
using System.Linq;

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

        public FileLoader FileLoader { get; }
        public GameModeInfo GameModeInfo { get; }
        public MapManager MapManager { get; private set; }
        public MapFragmentManager MapFragmentManager { get; private set; }

        public PokemonFactory PokemonFactory { get; private set; }

        public bool IsValid { get; private set; }

        /// <summary>
        /// Creates an instance of the <see cref="GameMode"/> class and loads the data model.
        /// </summary>
        public GameMode(GameModeInfo gameModeInfo, FileLoader fileLoader)
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
                PrimitivesFilePath,
                NaturesFilePath,
                TypesFilePath,
                MoveFilesPath
            }, OnLoadFinished);
        }

        private void OnLoadFinished(byte[][] data)
        {
            
        }

        public GeometryData GetPrimitiveData(string primitiveName)
        {
            throw new NotImplementedException();
            //return _primitiveModels.SingleOrDefault(p => p.Id == primitiveName);
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
    }
}
