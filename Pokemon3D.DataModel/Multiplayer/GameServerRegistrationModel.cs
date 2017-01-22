using System.Runtime.Serialization;

namespace Pokemon3D.DataModel.Multiplayer
{
    [DataContract(Namespace = "")]
    public class GameServerRegistrationModel
    {
        [DataMember(Order = 0)]
        public string Name;

        [DataMember(Order = 1)]
        public string IpAddress;
    }
}