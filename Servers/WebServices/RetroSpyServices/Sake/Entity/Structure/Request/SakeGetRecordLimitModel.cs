using System;
using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Request
{
    //<ns1:GetRecordLimit>
    //           <ns1:gameid>
    //               0
    //               </ns1:gameid>
    //           <ns1:secretKey>
    //               HA6zkS
    //               </ns1:secretKey>
    //           <ns1:loginTicket>
    //               3b7771ac0ae7451a8972d6__
    //               </ns1:loginTicket>
    //           <ns1:tableid>
    //               nicks
    //               </ns1:tableid>
    //           </ns1:GetRecordLimit>
    [DataContract(Namespace = SakeXmlLable.SakeNameSpace)]
    public class SakeGetRecordLimitRequest : SakeRequestBase
    {
    }
}
