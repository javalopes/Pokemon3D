using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.General
{
    /// <summary>
    /// The data model for a range.
    /// </summary>
    [DataContract(Namespace = "")]
    public class RangeModel : DataModel<RangeModel>
    {
        /// <summary>
        /// The lower bound of the range.
        /// </summary>
        [DataMember(Order = 0)]
        public double Min;

        /// <summary>
        /// The upper bound of the range.
        /// </summary>
        [DataMember(Order = 1)]
        public double Max;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
