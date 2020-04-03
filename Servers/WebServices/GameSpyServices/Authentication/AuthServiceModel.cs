using System.Runtime.Serialization;
namespace PublicServices.Authentication
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