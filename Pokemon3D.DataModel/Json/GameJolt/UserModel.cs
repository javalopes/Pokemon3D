using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameJolt
{
    [DataContract]
    public class UserModel : DataModel<UserModel>
    {
        [DataMember(Name = "id")]
        public string Id;

        [DataMember(Name = "type")]
        public string Type;

        [DataMember(Name = "username")]
        public string Username;

        [DataMember(Name = "avatar_url")]
        public string AvatarUrl;

        [DataMember(Name = "signed_up")]
        public string SignedUp;

        [DataMember(Name = "last_logged_in")]
        public string LastLoggedIn;

        [DataMember(Name = "status")]
        public string Status;

        [DataMember(Name = "developer_name")]
        public string DeveloperName;

        [DataMember(Name = "developer_website")]
        public string DeveloperWebsite;

        [DataMember(Name = "developer_description")]
        public string DeveloperDescription;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
