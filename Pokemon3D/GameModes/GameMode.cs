using Pokemon3D.DataModel;
using Pokemon3D.GameModes.Maps;
using Pokemon3D.GameModes.Pokemon;
using Pokemon3D.GameModes.Resources;
using Pokemon3D.Rendering.Data;
using System;

namespace Pokemon3D.GameModes
{
    /// <summary>
    /// Contains methods and members that control a GameMode, a collection of maps, scripts and assets.
    /// </summary>
    partial class GameMode : IDataModelContainer, IDisposable, GameModeDataProvider
    {
        public GameModeInfo GameModeInfo { get; }

        public MapManager MapManager { get; private set; }

        public MapFragmentManager MapFragmentManager { get; private set; }

        public PrimitiveManager PrimitiveManager { get; private set; }
        
        public NatureManager NatureManager { get; private set; }

        public TypeManager TypeManager { get; private set; }

        public PokemonFactory PokemonFactory { get; private set; }

        public MoveManager MoveManager { get; private set; }

        public bool IsValid { get; private set; }

        /// <summary>
        /// Creates an instance of the <see cref="GameMode"/> class and loads the data model.
        /// </summary>
        public GameMode(GameModeInfo gameModeInfo)
        {
            GameModeInfo = gameModeInfo;

            // only continue if the game mode config file loaded correctly.
            if (GameModeInfo.IsValid)
            {
                PrimitiveManager = new PrimitiveManager(this);
                MapManager = new MapManager(this);
                MapFragmentManager = new MapFragmentManager(this);
                NatureManager = new NatureManager(this);
                PokemonFactory = new PokemonFactory(this);
                TypeManager = new TypeManager(this);
                MoveManager = new MoveManager(this);
            }

            IsValid = true;
        }
        
        /// <summary>
        /// Starts to load the initial data from files like Pokémon Natures/Types etc.
        /// </summary>
        public void LoadInitialData()
        {
            PrimitiveManager.Start();
            NatureManager.Start();
            TypeManager.Start();
            MoveManager.Start();
        }

        /// <summary>
        /// Returns if all components of this GameMode have finished loading their data.
        /// </summary>
        public bool FinishedLoading
        {
            get
            {
                return NatureManager.FinishedLoading &&
                    TypeManager.FinishedLoading && 
                    MoveManager.FinishedLoading;
            }
        }

        public GeometryData GetPrimitiveData(string primitiveName)
        {
            return PrimitiveManager.GetPrimitiveData(primitiveName);
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
