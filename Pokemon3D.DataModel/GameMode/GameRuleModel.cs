﻿using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode
{
    [DataContract(Namespace = "")]
    public class GameRuleModel : DataModel<GameRuleModel>
    {
        [DataMember(Order = 0)]
        public string Id;

        [DataMember(Order = 1)]
        public string Value;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
