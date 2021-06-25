using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Request
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
    [DataContract(Namespace = SakeXmlLabel.SakeNameSpace)]
    public class SakeCreateRecordRequest : SakeRequestBase
    {
        [DataMember(Name = SakeXmlLabel.Values)]
        public Values Values;
    }

    [DataContract(Namespace = SakeXmlLabel.SakeNameSpace)]
    public class Values
    {
        [DataMember(Name = SakeXmlLabel.RecordField)]
        public RecordField RecordFieldList { get; set; }
    }

    [DataContract(Namespace = SakeXmlLabel.SakeNameSpace)]
    public class RecordField
    {
        [DataMember(Name = SakeXmlLabel.Name)]
        public string Name { get; set; }
        [DataMember(Name = SakeXmlLabel.Value)]
        public Value Value { get; set; }
    }

    [DataContract(Namespace = SakeXmlLabel.SakeNameSpace)]
    public class Value
    {
        [DataMember(Name = SakeXmlLabel.AsciiStringValue)]
        public AsciiStringValue AsciiStringValue { get; set; }
    }

    [DataContract(Namespace = SakeXmlLabel.SakeNameSpace)]
    public class AsciiStringValue
    {
        [DataMember(Name = SakeXmlLabel.Value)]
        public string Value { get; set; }
    }
}
