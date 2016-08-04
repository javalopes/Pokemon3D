using System;
using System.Collections.Generic;

namespace Assets.Editor.Data
{
    [Serializable]
    public class Vector3Model
    {
        public float X;

        public float Y;

        public float Z;
    }

    [Serializable]
    public class BattleMapDataModel
    {
        public string BattleMapFile;

        public Vector3Model CameraPosition;
    }

    [Serializable]
    public class EntityFieldPositionModel
    {
        public Vector3Model Position;

        public Vector3Model Size;

        public bool Fill;

        public Vector3Model Steps;

        public Vector3Model Rotation;

        public bool CardinalRotation;

        public Vector3Model Scale;
    }

    [Serializable]
    public class EntityComponentDataItemModel
    {
        public string Key;

        public string Value;
    }

    [Serializable]
    public class EntityComponentModel
    {
        public string Id;

        public List<EntityComponentDataItemModel> Data = new List<EntityComponentDataItemModel>();
    }

    [Serializable]
    public class EntityModel 
    {
        public string Id;

        public string Generator;

        public List<EntityComponentModel> Components = new List<EntityComponentModel>();

        public bool IsStatic;
    }

    [Serializable]
    public class EntityFieldModel
    {
        public List<EntityFieldPositionModel> Placing = new List<EntityFieldPositionModel>();

        public EntityModel Entity;
    }

    [Serializable]
    public class MapModel
    {
        public string Name;

        public string Id;

        public string Region;

        public string Zone;

        public string Song;

        public string MapScript;

        public string Environment;

        public string[] RandomNPCSources;

        public BattleMapDataModel BattleMapData;

        public List<EntityFieldModel> Entities = new List<EntityFieldModel>();

        public string[] Fragments;

        public string[] OffsetMaps;
    }
}
