using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Model
{
    [DataContract]
    public class SakeCreateRecordModel
    {
        [DataMember]
        public string param1 { get; set; }
    }
}
