using System.Runtime.Serialization;
namespace RetroSpyServices.Authentication.Entity.Structure.Model
{
    [DataContract]
    public class AuthServiceModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Email { get; set; }
    }
}