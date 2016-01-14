using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameJolt
{
    [DataContract(Namespace = "")]
    public class FriendModel : DataModel<FriendModel>
    {
        [DataMember(Name = "friend_id")]
        public string FriendId;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
