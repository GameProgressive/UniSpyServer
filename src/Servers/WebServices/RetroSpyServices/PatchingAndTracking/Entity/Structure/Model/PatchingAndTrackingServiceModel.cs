using System.Runtime.Serialization;
namespace RetroSpyServices.PatchingAndTracking.Entity.Structure.Model
{
    [DataContract]
    public class PatchingAndTrackingServiceModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Email { get; set; }
    }
}