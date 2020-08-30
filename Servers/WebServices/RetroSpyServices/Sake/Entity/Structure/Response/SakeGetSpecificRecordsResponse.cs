using System;
using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Response
{
    [DataContract(Name = "GetSpecificRecords")]
    public class SakeGetSpecificRecordsResponse
    {
        [DataMember(Name = "score")]
        public string Score;
    }
}
