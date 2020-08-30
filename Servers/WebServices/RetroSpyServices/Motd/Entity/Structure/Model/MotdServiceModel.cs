using System.Runtime.Serialization;
namespace RetroSpyServices.Motd.Entity.Structure.Model
{
    [DataContract]
    public class MotdServiceModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Email { get; set; }
    }
}