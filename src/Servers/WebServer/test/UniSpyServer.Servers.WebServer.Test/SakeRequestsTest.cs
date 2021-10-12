using System;
using Xunit;
using WebServer.Entity.Structure.Request.SakeRequest;
namespace UniSpyServer.Servers.WebServer.RequestTest
{
    public class SakeRequestsTest
    {
        //
        // These are the SOAP requests of SAKE
        // Endpoint: {FQDN}/sake/
        //
        //
        // These are the complete SOAP requests of "saketest"
        //
        [Fact]
        public void GetRecordLimit()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:GetRecordLimit>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:secretKey>XXXXXX</ns1:secretKey>
                            <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                            <ns1:tableid>nicks</ns1:tableid>
                        </ns1:GetRecordLimit>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new GetRecordLimitRequest(rawRequest);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("nicks", request.TableId);
        }
        [Fact]
        public void RateRecord()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:RateRecord>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:secretKey>XXXXXX</ns1:secretKey>
                            <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                            <ns1:tableid>test</ns1:tableid>
                            <ns1:recordid>158</ns1:recordid>
                            <ns1:rating>200</ns1:rating>
                        </ns1:RateRecord>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new RateRecordRequest(rawRequest);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("test", request.TableId);
            Assert.Equal("158", request.RecordId);
            Assert.Equal("200", request.Rating);
        }
        [Fact]
        public void GetRandomRecords()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:GetRandomRecords>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:secretKey>XXXXXX</ns1:secretKey>
                            <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                            <ns1:tableid>levels</ns1:tableid>
                            <ns1:max>1</ns1:max>
                            <ns1:fields>
                                <ns1:string>recordid</ns1:string>
                                <ns1:string>score</ns1:string>
                            </ns1:fields>
                        </ns1:GetRandomRecords>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new GetRandomRecordsRequest(rawRequest);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("levels", request.TableId);
            Assert.Equal("1", request.Max);
            Assert.Equal("recordid", request.Fields[0].FieldName);
            Assert.Equal("string", request.Fields[0].FiledType);
            Assert.Equal("score", request.Fields[1].FieldName);
            Assert.Equal("string", request.Fields[1].FiledType);
        }
        [Fact]
        public void GetSpecificRecords()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:GetSpecificRecords>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:secretKey>XXXXXX</ns1:secretKey>
                            <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                            <ns1:tableid>scores</ns1:tableid>
                            <ns1:recordids>
                                <ns1:int>1</ns1:int>
                                <ns1:int>2</ns1:int>
                                <ns1:int>4</ns1:int>
                                <ns1:int>5</ns1:int>
                            </ns1:recordids>
                            <ns1:fields>
                                <ns1:string>recordid</ns1:string>
                                <ns1:string>ownerid</ns1:string>
                                <ns1:string>score</ns1:string>
                            </ns1:fields>
                        </ns1:GetSpecificRecords>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new GetSpecificRecordsRequest(rawRequest);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("scores", request.TableId);
            Assert.Equal("1", request.RecordIds[0].FieldName);
            Assert.Equal("int", request.RecordIds[0].FiledType);
            Assert.Equal("2", request.RecordIds[1].FieldName);
            Assert.Equal("int", request.RecordIds[1].FiledType);
            Assert.Equal("4", request.RecordIds[2].FieldName);
            Assert.Equal("int", request.RecordIds[2].FiledType);
            Assert.Equal("5", request.RecordIds[3].FieldName);
            Assert.Equal("int", request.RecordIds[3].FiledType);
            Assert.Equal("recordid", request.Fields[0].FieldName);
            Assert.Equal("string", request.Fields[0].FiledType);
            Assert.Equal("ownerid", request.Fields[1].FieldName);
            Assert.Equal("string", request.Fields[1].FiledType);
            Assert.Equal("score", request.Fields[2].FieldName);
            Assert.Equal("string", request.Fields[2].FiledType);
        }
        [Fact]
        public void GetMyRecords()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:GetMyRecords>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:secretKey>XXXXXX</ns1:secretKey>
                            <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                            <ns1:tableid>test</ns1:tableid>
                            <ns1:fields>
                                <ns1:string>recordid</ns1:string>
                                <ns1:string>ownerid</ns1:string>
                                <ns1:string>MyByte</ns1:string>
                                <ns1:string>MyShort</ns1:string>
                                <ns1:string>MyInt</ns1:string>
                                <ns1:string>MyFloat</ns1:string>
                                <ns1:string>MyAsciiString</ns1:string>
                                <ns1:string>MyUnicodeString</ns1:string>
                                <ns1:string>MyBoolean</ns1:string>
                                <ns1:string>MyDateAndTime</ns1:string>
                                <ns1:string>MyBinaryData</ns1:string>
                                <ns1:string>MyFileID</ns1:string>
                                <ns1:string>num_ratings</ns1:string>
                                <ns1:string>average_rating</ns1:string>
                            </ns1:fields>
                        </ns1:GetMyRecords>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new GetMyRecordsRequest(rawRequest);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("test", request.TableId);
            Assert.Equal("recordid", request.Fields[0].FieldName);
            Assert.Equal("string", request.Fields[0].FiledType);
            Assert.Equal("ownerid", request.Fields[1].FieldName);
            Assert.Equal("string", request.Fields[1].FiledType);
            Assert.Equal("MyByte", request.Fields[2].FieldName);
            Assert.Equal("string", request.Fields[2].FiledType);
            Assert.Equal("MyShort", request.Fields[3].FieldName);
            Assert.Equal("string", request.Fields[3].FiledType);
            Assert.Equal("MyInt", request.Fields[4].FieldName);
            Assert.Equal("string", request.Fields[4].FiledType);
            Assert.Equal("MyFloat", request.Fields[5].FieldName);
            Assert.Equal("string", request.Fields[5].FiledType);
            Assert.Equal("MyAsciiString", request.Fields[6].FieldName);
            Assert.Equal("string", request.Fields[6].FiledType);
            Assert.Equal("MyUnicodeString", request.Fields[7].FieldName);
            Assert.Equal("string", request.Fields[7].FiledType);
            Assert.Equal("MyBoolean", request.Fields[8].FieldName);
            Assert.Equal("string", request.Fields[8].FiledType);
            Assert.Equal("MyDateAndTime", request.Fields[9].FieldName);
            Assert.Equal("string", request.Fields[9].FiledType);
            Assert.Equal("MyBinaryData", request.Fields[10].FieldName);
            Assert.Equal("string", request.Fields[10].FiledType);
            Assert.Equal("MyFileID", request.Fields[11].FieldName);
            Assert.Equal("string", request.Fields[11].FiledType);
            Assert.Equal("num_ratings", request.Fields[12].FieldName);
            Assert.Equal("string", request.Fields[12].FiledType);
            Assert.Equal("average_rating", request.Fields[13].FieldName);
            Assert.Equal("string", request.Fields[13].FiledType);
        }
        [Fact]
        public void SearchForRecords()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:SearchForRecords>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:secretKey>XXXXXX</ns1:secretKey>
                            <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                            <ns1:tableid>scores</ns1:tableid>
                            <ns1:filter></ns1:filter>
                            <ns1:sort></ns1:sort>
                            <ns1:offset>0</ns1:offset>
                            <ns1:max>3</ns1:max>
                            <ns1:surrounding>0</ns1:surrounding>
                            <ns1:ownerids></ns1:ownerids>
                            <ns1:cacheFlag>0</ns1:cacheFlag>
                            <ns1:fields>
                                <ns1:string>score</ns1:string>
                                <ns1:string>recordid</ns1:string>
                            </ns1:fields>
                        </ns1:SearchForRecords>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new SearchForRecordsRequest(rawRequest);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("scores", request.TableId);
            Assert.Equal("", request.Filter);
            Assert.Equal("", request.Sort);
            Assert.Equal("0", request.Offset);
            Assert.Equal("3", request.Max);
            Assert.Equal("0", request.Surrounding);
            Assert.Equal("", request.OwnerIds);
            Assert.Equal("0", request.CacheFlag);
            Assert.Equal("score", request.Fields[0].FieldName);
            Assert.Equal("string", request.Fields[0].FiledType);
            Assert.Equal("recordid", request.Fields[1].FieldName);
            Assert.Equal("string", request.Fields[1].FiledType);
        }
        [Fact]
        public void DeleteRecord()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:DeleteRecord>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:secretKey>XXXXXX</ns1:secretKey>
                            <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                            <ns1:tableid>test</ns1:tableid>
                            <ns1:recordid>150</ns1:recordid>
                        </ns1:DeleteRecord>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new DeleteRecordRequest(rawRequest);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("test", request.TableId);
            Assert.Equal("150", request.RecordId);
        }
        //
        // TODO: Deserialization of RecordFields
        //
        [Fact]
        public void UpdateRecord()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:UpdateRecord>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:secretKey>XXXXXX</ns1:secretKey>
                            <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                            <ns1:tableid>test</ns1:tableid>
                            <ns1:recordid>158</ns1:recordid>
                            <ns1:values>
                                <ns1:RecordField>
                                    <ns1:name>MyByte</ns1:name>
                                    <ns1:value>
                                        <ns1:byteValue>
                                            <ns1:value>123</ns1:value>
                                        </ns1:byteValue>
                                    </ns1:value>
                                </ns1:RecordField>
                                <ns1:RecordField>
                                    <ns1:name>MyShort</ns1:name>
                                    <ns1:value>
                                        <ns1:shortValue>
                                            <ns1:value>12345</ns1:value>
                                        </ns1:shortValue>
                                    </ns1:value>
                                </ns1:RecordField>
                                <ns1:RecordField>
                                    <ns1:name>MyInt</ns1:name>
                                    <ns1:value>
                                        <ns1:intValue>
                                            <ns1:value>123456789</ns1:value>
                                        </ns1:intValue>
                                    </ns1:value>
                                </ns1:RecordField>
                                <ns1:RecordField>
                                    <ns1:name>MyFloat</ns1:name>
                                    <ns1:value>
                                        <ns1:floatValue>
                                            <ns1:value>3.141593</ns1:value>
                                        </ns1:floatValue>
                                    </ns1:value>
                                </ns1:RecordField>
                                <ns1:RecordField>
                                    <ns1:name>MyAsciiString</ns1:name>
                                    <ns1:value>
                                        <ns1:asciiStringValue>
                                            <ns1:value>ascii</ns1:value>
                                        </ns1:asciiStringValue>
                                    </ns1:value>
                                </ns1:RecordField>
                                <ns1:RecordField>
                                    <ns1:name>MyUnicodeString</ns1:name>
                                    <ns1:value>
                                        <ns1:unicodeStringValue>
                                            <ns1:value>unicode</ns1:value>
                                        </ns1:unicodeStringValue>
                                    </ns1:value>
                                </ns1:RecordField>
                                <ns1:RecordField>
                                    <ns1:name>MyBoolean</ns1:name>
                                    <ns1:value>
                                        <ns1:booleanValue>
                                            <ns1:value>1</ns1:value>
                                        </ns1:booleanValue>
                                    </ns1:value>
                                </ns1:RecordField>
                                <ns1:RecordField>
                                    <ns1:name>MyDateAndTime</ns1:name>
                                    <ns1:value>
                                        <ns1:dateAndTimeValue>
                                            <ns1:value>2020-05-21T11:13:41Z</ns1:value>
                                        </ns1:dateAndTimeValue>
                                    </ns1:value>
                                </ns1:RecordField>
                                <ns1:RecordField>
                                    <ns1:name>MyBinaryData</ns1:name>
                                    <ns1:value>
                                        <ns1:binaryDataValue>
                                            <ns1:value>EjRWq80=</ns1:value>
                                        </ns1:binaryDataValue>
                                    </ns1:value>
                                </ns1:RecordField>
                            </ns1:values>
                        </ns1:UpdateRecord>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new UpdateRecordRequest(rawRequest);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("test", request.TableId);
            Assert.Equal("158", request.RecordId);

            // Assert.Equal("score", request.Values[0].FieldName);
            // Assert.Equal("string", request.Values[0].FiledType);
            // Assert.Equal("recordid", request.Values[1].FieldName);
            // Assert.Equal("string", request.Values[1].FiledType);
        }
        //
        // TODO: Deserialization of RecordFields
        //
        [Fact]
        public void CreateRecord()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:CreateRecord>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:secretKey>XXXXXX</ns1:secretKey>
                            <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                            <ns1:tableid>test</ns1:tableid>
                            <ns1:values>
                                <ns1:RecordField>
                                    <ns1:name>MyAsciiString</ns1:name>
                                    <ns1:value>
                                        <ns1:asciiStringValue>
                                            <ns1:value>this is a record</ns1:value>
                                        </ns1:asciiStringValue>
                                    </ns1:value>
                                </ns1:RecordField>
                            </ns1:values>
                        </ns1:CreateRecord>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new CreateRecordRequest(rawRequest);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("test", request.TableId);

            // Assert.Equal("score", request.Values[0].FieldName);
            // Assert.Equal("string", request.Values[0].FiledType);
            // Assert.Equal("recordid", request.Values[1].FieldName);
            // Assert.Equal("string", request.Values[1].FiledType);
        }
    }
}
