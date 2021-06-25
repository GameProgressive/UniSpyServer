using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Response
{
    [DataContract(Name = "CreateRecord")]
    public class SakeCreateRecordResponse
    {
        [DataMember(Name = SakeXmlLabel.RecordID)]
        public uint RecordID;

    }
}
