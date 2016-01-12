using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameJolt
{
    [DataContract]
    public class BatchResponseModel : DataModel<BatchResponseModel>
    {
        [DataMember(Order = 0, Name = "success")]
        public bool Success;
        
        [DataMember(Order = 1, Name = "responses")]
        public CallResponseModel[] Responses;

        public override object Clone()
        {
            var clone = (BatchResponseModel)MemberwiseClone();
            clone.Responses = (CallResponseModel[])Responses.Clone();
            return clone;
        }
    }
}
