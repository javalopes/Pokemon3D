﻿using System.Runtime.Serialization;
using Pokemon3D.DataModel.General;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Definitions
{
    /// <summary>
    /// The data model for a Pokémon or move type in the game.
    /// </summary>
    [DataContract(Namespace = "")]
    public class TypeModel : DataModel<TypeModel>
    {
        /// <summary>
        /// The identification of the type.
        /// </summary>
        [DataMember(Order = 0)]
        public string Id;

        /// <summary>
        /// The name of the type.
        /// </summary>
        [DataMember(Order = 1)]
        public string Name;

        /// <summary>
        /// The color of this type.
        /// </summary>
        [DataMember(Order = 2)]
        public ColorModel Color;

        /// <summary>
        /// The type texture of this type.
        /// </summary>
        [DataMember(Order = 3)]
        public TextureSourceModel Texture;

        /// <summary>
        /// The list of types moves of this type are effective against.
        /// </summary>
        [DataMember(Order = 4)]
        public string[] Effective;

        /// <summary>
        /// The list of types moves of this type are not effective against.
        /// </summary>
        [DataMember(Order = 5)]
        public string[] Ineffective;

        /// <summary>
        /// The list of types moves of this type have no effect on.
        /// </summary>
        [DataMember(Order = 6)]
        public string[] Unaffected;

        public override object Clone()
        {
            var clone = (TypeModel)MemberwiseClone();
            clone.Color = Color.CloneModel();
            clone.Texture = Texture.CloneModel();
            clone.Effective = (string[])Effective.Clone();
            clone.Ineffective = (string[])Ineffective.Clone();
            clone.Unaffected = (string[])Unaffected.Clone();
            return clone;
        }
    }
}