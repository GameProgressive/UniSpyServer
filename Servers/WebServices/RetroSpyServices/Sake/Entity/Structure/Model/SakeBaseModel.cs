using System;
using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Model
{
    public class SakeBaseModel
    {
        [DataMember(Name = SakeConstant.GameID)]
        public string GameID { get; set; }

        [DataMember(Name = SakeConstant.SecretKey)]
        public string SecretKey { get; set; }

        [DataMember(Name = SakeConstant.LoginTicket)]
        public string LoginTicket { get; set; }

        [DataMember(Name = SakeConstant.TableID)]
        public string TableID { get; set; }

        [DataMember(Name = SakeConstant.RecordID)]
        public uint RecordID { get; set; }
    }
}
