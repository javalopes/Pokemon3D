using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Requests
{
    /// <summary>
    /// Name and content of a file.
    /// </summary>
    [DataContract(Namespace = "")]
    public class FileContentModel : DataModel<FileContentModel>
    {
        [DataMember(Order = 0)]
        public string FileName;

        [DataMember(Order = 1)]
        public string FileContent;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
