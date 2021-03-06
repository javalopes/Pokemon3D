﻿using System.Runtime.Serialization;
using Pokemon3D.DataModel.General;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Definitions
{
    /// <summary>
    /// The data model for a body style of a Pokémon.
    /// </summary>
    [DataContract(Namespace = "")]
    public class BodyStyleModel : DataModel<BodyStyleModel>
    {
        /// <summary>
        /// The identification of this BodyStyle.
        /// </summary>
        [DataMember(Order = 0)]
        public string Id;

        /// <summary>
        /// The name of this BodyStyle.
        /// </summary>
        [DataMember(Order = 1)]
        public string Name;

        /// <summary>
        /// The texture of this BodyStyle.
        /// </summary>
        [DataMember(Order = 2)]
        public TextureSourceModel Texture;

        public override object Clone()
        {
            var clone = (BodyStyleModel)MemberwiseClone();
            clone.Texture = Texture.CloneModel();
            return clone;
        }
    }
}
