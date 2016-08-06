using System;
using System.Collections.Generic;

namespace Assets.Editor.Data
{
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
