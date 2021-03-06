﻿using System.Runtime.Serialization;
using Pokemon3D.DataModel.General;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Definitions.World
{
    /// <summary>
    /// A data model for the fly destination used by a map object.
    /// </summary>
    [DataContract(Namespace = "")]
    public class FlyToModel : DataModel<FlyToModel>
    {
        [DataMember(Order = 0)]
        public Vector3Model Position;

        [DataMember(Order = 1)]
        public string Mapfile;

        public override object Clone()
        {
            var clone = (FlyToModel)MemberwiseClone();
            clone.Position = Position.CloneModel();
            return clone;
        }
    }
}
