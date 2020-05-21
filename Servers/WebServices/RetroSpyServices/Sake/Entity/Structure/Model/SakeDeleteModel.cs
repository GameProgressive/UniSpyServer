using System;
using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Model
{
    [DataContract]
    public class SakeDeleteModel
    {
        [DataMember(Name = "DeleteRecord")]
        public string DeleteRecord { get; set; }

        [DataMember(Name = "ns1:DeleteRecord")]
        public string ns1DeleteRecord { get; set; }

    }
}
