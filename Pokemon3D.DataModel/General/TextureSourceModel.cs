﻿using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.General
{
    /// <summary>
    /// Describes a texture source.
    /// </summary>
    [DataContract(Namespace = "")]
    public class TextureSourceModel : DataModel<TextureSourceModel>
    {
        /// <summary>
        /// The source file for this texture.
        /// </summary>
        [DataMember(Order = 0)]
        public string Source;

        /// <summary>
        /// The rectangle cut out of the source texture.
        /// </summary>
        [DataMember(Order = 1)]
        public RectangleModel Rectangle;

        public override object Clone()
        {
            var clone = (TextureSourceModel)MemberwiseClone();
            clone.Rectangle = Rectangle.CloneModel();
            return clone;
        }
    }
}
