﻿using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokémon3D.DataModel.Json.GameMode.Map
{
    /// <summary>
    /// A data model for a map.
    /// </summary>
    [DataContract]
    class MapModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string Name { get; private set; }

        [DataMember(Order = 1)]
        public string Region { get; private set; }

        [DataMember(Order = 2)]
        public string Zone { get; private set; }

        [DataMember(Order = 3)]
        public string Song { get; private set; }

        [DataMember(Order = 4)]
        public string MapScript { get; private set; }

        [DataMember(Order = 5)]
        public string Environment { get; private set; }

        [DataMember(Order = 6)]
        public BattleMapDataModel BattleMapData { get; private set; }

        [DataMember(Order = 7)]
        public Entities.EntityFieldModel[] Entities { get; private set; }

        [DataMember(Order = 8)]
        public OffsetMapModel[] OffsetMaps { get; private set; }
    }
}
