using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Definitions.World
{
    /// <summary>
    /// The data model for a route object.
    /// </summary>
    [DataContract(Namespace = "")]
    public class RouteModel : MapObjectModel
    {
        #region RouteType

        [DataMember(Order = 4, Name = "RouteType")]
        private string _routeType;
        
        public RouteType RouteType => ConvertStringToEnum<RouteType>(_routeType);

        #endregion

        #region RouteDirection

        [DataMember(Order = 5, Name = "RouteDirection")]
        private string _routeDirection;
        
        public RouteDirection RouteDirection => ConvertStringToEnum<RouteDirection>(_routeDirection);

        #endregion

        public override object Clone()
        {
            var clone = (RouteModel)MemberwiseClone();
            clone.Position = Position.CloneModel();
            clone.FlyTo = FlyTo.CloneModel();
            return clone;
        }
    }
}
