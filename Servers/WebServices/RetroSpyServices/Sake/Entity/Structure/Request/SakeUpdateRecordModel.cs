﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Request
{
    //<ns1:UpdateRecord>
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
    //                test
    //                </ns1:tableid>
    //            <ns1:recordid>
    //                158
    //                </ns1:recordid>
    //            <ns1:values>
    //                <ns1:RecordField>
    //                    <ns1:name>
    //                        MyByte
    //                        </ns1:name>
    //                    <ns1:value>
    //                        <ns1:byteValue>
    //                            <ns1:value>
    //                                123
    //                                </ns1:value>
    //                            </ns1:byteValue>
    //                        </ns1:value>
    //                    </ns1:RecordField>
    //                <ns1:RecordField>
    //                    <ns1:name>
    //                        MyShort
    //                        </ns1:name>
    //                    <ns1:value>
    //                        <ns1:shortValue>
    //                            <ns1:value>
    //                                12345
    //                                </ns1:value>
    //                            </ns1:shortValue>
    //                        </ns1:value>
    //                    </ns1:RecordField>
    //                <ns1:RecordField>
    //                    <ns1:name>
    //                        MyInt
    //                        </ns1:name>
    //                    <ns1:value>
    //                        <ns1:intValue>
    //                            <ns1:value>
    //                                123456789
    //                                </ns1:value>
    //                            </ns1:intValue>
    //                        </ns1:value>
    //                    </ns1:RecordField>
    //                <ns1:RecordField>
    //                    <ns1:name>
    //                        MyFloat
    //                        </ns1:name>
    //                    <ns1:value>
    //                        <ns1:floatValue>
    //                            <ns1:value>
    //                                3.141593
    //                                </ns1:value>
    //                            </ns1:floatValue>
    //                        </ns1:value>
    //                    </ns1:RecordField>
    //                <ns1:RecordField>
    //                    <ns1:name>
    //                        MyAsciiString
    //                        </ns1:name>
    //                    <ns1:value>
    //                        <ns1:asciiStringValue>
    //                            <ns1:value>
    //                                ascii
    //                                </ns1:value>
    //                            </ns1:asciiStringValue>
    //                        </ns1:value>
    //                    </ns1:RecordField>
    //                <ns1:RecordField>
    //                    <ns1:name>
    //                        MyUnicodeString
    //                        </ns1:name>
    //                    <ns1:value>
    //                        <ns1:unicodeStringValue>
    //                            <ns1:value>
    //                                unicode
    //                                </ns1:value>
    //                            </ns1:unicodeStringValue>
    //                        </ns1:value>
    //                    </ns1:RecordField>
    //                <ns1:RecordField>
    //                    <ns1:name>
    //                        MyBoolean
    //                        </ns1:name>
    //                    <ns1:value>
    //                        <ns1:booleanValue>
    //                            <ns1:value>
    //                                1
    //                                </ns1:value>
    //                            </ns1:booleanValue>
    //                        </ns1:value>
    //                    </ns1:RecordField>
    //                <ns1:RecordField>
    //                    <ns1:name>
    //                        MyDateAndTime
    //                        </ns1:name>
    //                    <ns1:value>
    //                        <ns1:dateAndTimeValue>
    //                            <ns1:value>
    //                                2020-05-21T11:13:41Z
    //                                </ns1:value>
    //                            </ns1:dateAndTimeValue>
    //                        </ns1:value>
    //                    </ns1:RecordField>
    //                <ns1:RecordField>
    //                    <ns1:name>
    //                        MyBinaryData
    //                        </ns1:name>
    //                    <ns1:value>
    //                        <ns1:binaryDataValue>
    //                            <ns1:value>
    //                                EjRWq80=
    //                                </ns1:value>
    //                            </ns1:binaryDataValue>
    //                        </ns1:value>
    //                    </ns1:RecordField>
    //                </ns1:values>
    //            </ns1:UpdateRecord>
    [DataContract(Namespace = SakeXmlLable.SakeNameSpace)]
    public class SakeUpdateRecordRequest : SakeRequestBase
    {
    }
}
