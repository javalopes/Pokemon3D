using System.Runtime.Serialization;

namespace Pokemon3D.DataModel.Multiplayer
{
    [DataContract(Namespace = "")]
    public class GameServerModel
    {
        [DataMember(Order = 0)]
        public int Id;

        [DataMember(Order = 1)]
        public string Name;

        [DataMember(Order = 2)]
        public string IpAddress;
    }
}
