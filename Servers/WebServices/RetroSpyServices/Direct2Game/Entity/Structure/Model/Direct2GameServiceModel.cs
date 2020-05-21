using System.Runtime.Serialization;
namespace RetroSpyServices.Direct2Game.Entity.Structure.Model
{
    [DataContract]
    public class Direct2GameServiceModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Email { get; set; }
    }
}