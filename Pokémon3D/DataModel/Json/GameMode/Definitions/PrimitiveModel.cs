using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokémon3D.DataModel.Json.GameMode.Definitions
{
    /// <summary>
    /// The data model for a primitive model.
    /// </summary>
    [DataContract]
    class PrimitiveModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string Name;
        
        [DataMember(Order = 1)]
        public VertexModel[] Vertices;
        
        [DataMember(Order = 2)]
        public int[] Indices;
    }
}
