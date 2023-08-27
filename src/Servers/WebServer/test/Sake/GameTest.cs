using Moq;
using UniSpy.Server.Core.Abstraction.Interface;
using Xunit;

namespace UniSpy.Server.WebServer.Test.Sake
{
    public class GameTest
    {
        [Fact]
        public void Crysis2SakeTest()
        {
            var raw = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                            <SOAP-ENV:Envelope
                                xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                                xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                                xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                                xmlns:ns1=""http://gamespy.net/sake"">
                                <SOAP-ENV:Body>
                                    <ns1:SearchForRecords>
                                        <ns1:gameid>3300</ns1:gameid>
                                        <ns1:secretKey>8TTq4M</ns1:secretKey>
                                        <ns1:loginTicket>0000000000000000000000__</ns1:loginTicket>
                                        <ns1:tableid>DEDICATEDSTATS</ns1:tableid>
                                        <ns1:filter>PROFILE&#x20;=&#x20;35</ns1:filter>
                                        <ns1:sort>recordid</ns1:sort>
                                        <ns1:offset>0</ns1:offset>
                                        <ns1:max>1</ns1:max>
                                        <ns1:surrounding>0</ns1:surrounding>
                                        <ns1:ownerids>2</ns1:ownerids>
                                        <ns1:cacheFlag>0</ns1:cacheFlag>
                                        <ns1:fields>
                                            <ns1:string>DATA</ns1:string>
                                            <ns1:string>recordid</ns1:string>
                                        </ns1:fields>
                                    </ns1:SearchForRecords>
                                </SOAP-ENV:Body>
                            </SOAP-ENV:Envelope>";

            var req = new Mock<IHttpRequest>();
            req.Setup(r => r.Body).Returns(raw);

            var switcher = new WebServer.Handler.CmdSwitcher(MokeObject.Client, req.Object);
            switcher.Handle();

        }
    }
}