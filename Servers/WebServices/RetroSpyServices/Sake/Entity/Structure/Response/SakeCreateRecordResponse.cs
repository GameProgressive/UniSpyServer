using System;
using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Response
{
    [DataContract(Name ="CreateRecord")]
    public class SakeCreateRecordResponse
    {
        [DataMember(Name = SakeXmlLable.RecordID)]
        public uint RecordID;

    }
}
