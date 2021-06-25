using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Request
{
    [DataContract(Namespace = SakeXmlLabel.SakeNameSpace)]
    public class SakeRequestBase
    {
        [DataMember(Name = SakeXmlLabel.GameID, Order = 0)]
        public string GameID { get; set; }

        [DataMember(Name = SakeXmlLabel.SecretKey, Order = 1)]
        public string SecretKey { get; set; }

        [DataMember(Name = SakeXmlLabel.LoginTicket, Order = 2)]
        public string LoginTicket { get; set; }

        [DataMember(Name = SakeXmlLabel.TableID, Order = 3)]
        public string TableID { get; set; }

        [DataMember(Name = SakeXmlLabel.RecordID, Order = 4)]
        public uint RecordID { get; set; }
    }
}
