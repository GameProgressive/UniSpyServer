using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.WebServer.Entity.Structure.Request.SakeRequest;
using WebServer.Handler.CmdHandler;
using Xunit;

namespace UniSpyServer.Servers.UniSpyServer.WebServer.Test.HandlerTest
{
    public class TestSession : IUniSpySession
    {
        public EndPoint RemoteEndPoint => null;
        public IPEndPoint RemoteIPEndPoint => null;

        public bool BaseSend(IUniSpyResponse response)
        {
            return true;
        }

        public bool Send(IUniSpyResponse response)
        {
            return true;
        }
    }
    public class SakeHandlerTest
    {
        [Fact]
        public void CreateRecordTest()
        {
            //Given
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
            var handler = new CreateRecordHandler(new TestSession(), request);
            handler.Handle();
            //When

            //Then
        }
    }
}