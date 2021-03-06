﻿using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.i18n
{
    [DataContract(Namespace = "")]
    public class TokenModel : DataModel<TokenModel>
    {
        [DataMember]
        public string Id;
        [DataMember]
        public string Val;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
