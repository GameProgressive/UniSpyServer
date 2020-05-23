using System.Runtime.Serialization;
namespace RetroSpyServices.Competitive.Entity.Structure.Model
{
    [DataContract]
    public class CompetitiveServiceModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Email { get; set; }
    }
}