using System;
using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Model.Request
{

    //<ns1:GetRandomRecords>
    //<ns1:gameid>
    //    0
    //    </ns1:gameid>
    //<ns1:secretKey>
    //    HA6zkS
    //    </ns1:secretKey>
    //<ns1:loginTicket>
    //    3b7771ac0ae7451a8972d6__
    //    </ns1:loginTicket>
    //<ns1:tableid>
    //    levels
    //    </ns1:tableid>
    //<ns1:max>
    //    1
    //    </ns1:max>
    //<ns1:fields>
    //    <ns1:string>
    //        recordid
    //        </ns1:string>
    //    <ns1:string>
    //        score
    //        </ns1:string>
    //    </ns1:fields>
    //</ns1:GetRandomRecords>
    [DataContract(Namespace = SakeXmlLable.SakeNameSpace)]
    public class SakeGetRamdomRecordRequest:SakeRequestBase
    {
        [DataMember(Name = SakeXmlLable.Max)]
        public uint Max;
    }
}
