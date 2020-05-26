using System;
using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Model
{
    public class SakeRequestBase
    {
        [DataMember(Name = SakeXmlLable.GameID)]
        public string GameID { get; set; }

        [DataMember(Name = SakeXmlLable.SecretKey)]
        public string SecretKey { get; set; }

        [DataMember(Name = SakeXmlLable.LoginTicket)]
        public string LoginTicket { get; set; }

        [DataMember(Name = SakeXmlLable.TableID)]
        public string TableID { get; set; }

        [DataMember(Name = SakeXmlLable.RecordID)]
        public uint RecordID { get; set; }
    }
}
