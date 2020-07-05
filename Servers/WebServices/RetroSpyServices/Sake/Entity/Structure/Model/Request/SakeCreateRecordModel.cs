﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Model.Basic;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Model.Request
{
    //    <SOAP-ENV:Body>
    //    <ns1:CreateRecord>
    //        <ns1:gameid>
    //            0
    //            </ns1:gameid>
    //        <ns1:secretKey>
    //            HA6zkS
    //            </ns1:secretKey>
    //        <ns1:loginTicket>
    //            3b7771ac0ae7451a8972d6__
    //            </ns1:loginTicket>
    //        <ns1:tableid>
    //            test
    //            </ns1:tableid>
    //        <ns1:values>
    //            <ns1:RecordField>
    //                <ns1:name>
    //                    MyAsciiString
    //                    </ns1:name>
    //                <ns1:value>
    //                    <ns1:asciiStringValue>
    //                        <ns1:value>
    //                            this&#x20;is&#x20;a&#x20;record
    //                            </ns1:value>
    //                        </ns1:asciiStringValue>
    //                    </ns1:value>
    //                </ns1:RecordField>
    //            </ns1:values>
    //        </ns1:CreateRecord>
    //    </SOAP-ENV:Body>
    //</SOAP-ENV:Envelope>
    [DataContract(Name = "CreateRecord"), KnownType(typeof(SakeRequestBase))]
    public class SakeCreateRecordRequest : SakeRequestBase
    {
        [DataMember(Name = SakeXmlLable.Values,Order = 5)]
        public string KeyValues;
    }
}
