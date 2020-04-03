using System.Runtime.Serialization;
namespace Sake
{
    [DataContract]
    public class StorageServerModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Email { get; set; }
    }
}