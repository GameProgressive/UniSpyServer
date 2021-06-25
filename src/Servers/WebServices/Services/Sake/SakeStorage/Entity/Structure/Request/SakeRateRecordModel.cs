using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Request
{
    [DataContract(Namespace = SakeXmlLabel.SakeNameSpace)]
    public class SakeRateRecordRequest : SakeRequestBase
    {
        [DataMember(Name = SakeXmlLabel.Rating)]
        public uint Rating;
    }
}
