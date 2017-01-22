﻿using System.Runtime.Serialization;
using Pokemon3D.DataModel.General;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Savegame.StorageSystem
{
    [DataContract(Namespace = "")]
    public class StorageBoxStyleModel : DataModel<StorageBoxStyleModel>
    {
        [DataMember(Order = 0)]
        public TextureSourceModel Background;

        [DataMember(Order = 1)]
        public TextureSourceModel Header;

        public override object Clone()
        {
            var clone = (StorageBoxStyleModel)MemberwiseClone();
            clone.Background = Background.CloneModel();
            clone.Header = Header.CloneModel();
            return clone;
        }
    }
}
