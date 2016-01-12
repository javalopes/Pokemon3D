using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameJolt
{
    [DataContract(Namespace = "")]
    public class ResponseModel : DataModel<ResponseModel>
    {
        [DataMember(Order = 0, Name = "response")]
        public BatchResponseModel Response;

        public override object Clone()
        {
            var clone = (ResponseModel)MemberwiseClone();
            clone.Response = Response.CloneModel();
            return clone;
        }
    }
}
