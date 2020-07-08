using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Model.Request
{
    [DataContract]
    public class SakeRequestBase
    {
        [DataMember(Name = SakeXmlLable.GameID, Order = 0)]
        public string GameID { get; set; }

        [DataMember(Name = SakeXmlLable.SecretKey, Order = 1)]
        public string SecretKey { get; set; }

        [DataMember(Name = SakeXmlLable.LoginTicket, Order = 2)]
        public string LoginTicket { get; set; }

        [DataMember(Name = SakeXmlLable.TableID, Order = 3)]
        public string TableID { get; set; }

        [DataMember(Name = SakeXmlLable.RecordID, Order = 4)]
        public uint RecordID { get; set; }
    }
}
