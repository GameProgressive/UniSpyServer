using System;
using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Model
{
    public class SakeRateRecordRequest : SakeRequestBase
    {
        [DataMember(Name = SakeXmlLable.Rating)]
        public uint Rating;
    }
}
