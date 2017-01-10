using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Pokemon3D.Common.FileSystem;
using Pokemon3D.DataModel.GameMode.Map;
using Pokemon3D.Entities.System;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes;
using static Pokemon3D.GameProvider;

namespace Pokemon3D.Entities
{
    internal class Map
    {
        private readonly World _world;
        private readonly List<Entity> _allMapEntities;
        private readonly string _dataPath;
        private readonly string _id;

        public MapModel Model { get; private set; }
        public bool IsActive { get; private set; }

        public Vector3 Offset { get; private set; }

        public Map(World world, string id)
        {
            _world = world;
            _id = id;
            _allMapEntities = new List<Entity>();
            var gameMode = GameInstance.GetService<GameModeManager>().ActiveGameMode;
            _dataPath = gameMode.GetMapFilePath(_id);
        }

        public void Load(Vector3 basicOffset, bool forceResetCache = false)
        {
            if (_allMapEntities.Count > 0) throw new InvalidOperationException("Map seems already loaded, call unload first.");

            var gameMode = GameInstance.GetService<GameModeManager>().ActiveGameMode;
            Model = gameMode.LoadMap(_id);

            Offset = basicOffset;
            if (Model.Entities != null)
            {
                foreach (var entityDefinition in Model.Entities)
                {
                    foreach (var entityPlacing in entityDefinition.Placing)
                    {
                        var entities = _world.PlaceEntities(entityDefinition, entityPlacing, basicOffset);
                        _allMapEntities.AddRange(entities);
                    }
                }
            }

            var mapFragmentManager = GameInstance.GetService<GameModeManager>().ActiveGameMode.MapFragmentManager;

            if (Model.Fragments != null)
            {
                foreach (var fragmentImport in Model.Fragments)
                {
                    var importedFragment = mapFragmentManager.GetFragment(fragmentImport.Id);

                    foreach (var position in fragmentImport.Positions)
                    {
                        var fragmentOffset = position.GetVector3() + basicOffset;

                        var entities = _world.CreateEntitiesFromFragment(importedFragment, fragmentOffset);
                        _allMapEntities.AddRange(entities);
                    }
                }
            }
            IsActive = true;

            if (GameInstance.GetService<GameConfiguration>().Data.EnableFileHotSwapping)
            {
                FileObserver.Instance.StartFileObserve(_dataPath, MapChanged);
            }
        }

        public void Unload()
        {
            foreach(var entity in _allMapEntities)
            {
                _world.EntitySystem.RemoveEntity(entity);
            }
            _allMapEntities.Clear();
        }

        public void Deactivate()
        {
            if (!IsActive) return;
            foreach(var entity in _allMapEntities)
            {
                entity.IsActive = false;
            }

            if (GameInstance.GetService<GameConfiguration>().Data.EnableFileHotSwapping)
            {
                FileObserver.Instance.StopFileObserve(_dataPath, MapChanged);
            }
            IsActive = false;
        }

        public void Activate()
        {
            if (IsActive) return;
            foreach (var entity in _allMapEntities)
            {
                entity.IsActive = true;
            }
            if (GameInstance.GetService<GameConfiguration>().Data.EnableFileHotSwapping)
            {
                FileObserver.Instance.StartFileObserve(_dataPath, MapChanged);
            }
            IsActive = true;
        }

        private void MapChanged(object sender, FileSystemEventArgs e)
        {
            var gameMode = GameInstance.GetService<GameModeManager>().ActiveGameMode;
            foreach (var entity in _allMapEntities)
            {
                _world.EntitySystem.RemoveEntity(entity);
            }
            _allMapEntities.Clear();
            Load(Offset, true);
            _world.EntitySystem.InitializeAllPendingEntities();
        }
    }
}
