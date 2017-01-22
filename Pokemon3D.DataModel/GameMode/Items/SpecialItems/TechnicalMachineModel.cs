﻿using System.Runtime.Serialization;
// ReSharper disable InconsistentNaming

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Items.SpecialItems
{
    [DataContract(Namespace = "")]
    public class TechnicalMachineModel : DataModel<TechnicalMachineModel>
    {
        [DataMember(Order = 0)]
        public int MoveId;

        [DataMember(Order = 1)]
        public bool IsHM;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
