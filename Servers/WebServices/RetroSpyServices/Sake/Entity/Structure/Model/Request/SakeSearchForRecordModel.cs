using System;
using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Model.Request
{
    //<ns1:SearchForRecords>
    //            <ns1:gameid>
    //                0
    //                </ns1:gameid>
    //            <ns1:secretKey>
    //                HA6zkS
    //                </ns1:secretKey>
    //            <ns1:loginTicket>
    //                3b7771ac0ae7451a8972d6__
    //                </ns1:loginTicket>
    //            <ns1:tableid>
    //                scores
    //                </ns1:tableid>
    //            <ns1:filter>
    //                score&#x20;&lt;&#x20;50
    //                </ns1:filter>
    //            <ns1:sort>
    //                score&#x20;desc
    //                </ns1:sort>
    //            <ns1:offset>
    //                0
    //                </ns1:offset>
    //            <ns1:max>
    //                3
    //                </ns1:max>
    //            <ns1:surrounding>
    //                0
    //                </ns1:surrounding>
    //            <ns1:ownerids>
    //                </ns1:ownerids>
    //            <ns1:cacheFlag>
    //                0
    //                </ns1:cacheFlag>
    //            <ns1:fields>
    //                <ns1:string>
    //                    score
    //                    </ns1:string>
    //                <ns1:string>
    //                    recordid
    //                    </ns1:string>
    //                </ns1:fields>
    //            </ns1:SearchForRecords>
    [DataContract(Namespace = SakeXmlLable.SakeNameSpace)]
    public class SakeSearchForRecordRequest : SakeRequestBase
    {
        [DataMember(Name = SakeXmlLable.Filter)]
        public string Filter;

        [DataMember(Name = SakeXmlLable.Sort)]
        public string Sort;

        [DataMember(Name = SakeXmlLable.Offset)]
        public uint Offset;

        [DataMember(Name = SakeXmlLable.Max)]
        public uint Max;

        [DataMember(Name = SakeXmlLable.Surrounding)]
        public uint Surrounding;

        [DataMember(Name = SakeXmlLable.OwnerID)]
        public uint OwnerID;

        [DataMember(Name = SakeXmlLable.CacheFlag)]
        public bool CacheFlag;

        //    <ns1:fields>
        //<ns1:string>
        //    DATA
        //    </ns1:string>
        //<ns1:string>
        //    recordid
        //    </ns1:string>
        //</ns1:fields>
    }
}
