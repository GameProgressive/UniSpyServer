using System;
using Xunit;
using WebServer.Entity.Structure.Request.AtlasRequest;
namespace UniSpyServer.Servers.WebServer.RequestTest
{
    public class AtlasRequestsTest
    {
        //
        // These are the SOAP requests of ATLAS (Competition)
        // Endpoint: {FQDN}/competition/
        //
        [Fact]
        public void CreateMatchlessSession()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:CreateMatchlessSession>
                            <ns1:certificate>XXXXXX</ns1:certificate>
                            <ns1:proof>XXXXXX</ns1:proof>
                            <ns1:gameid>0</ns1:gameid>
                        </ns1:CreateMatchlessSession>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new CreateMatchlessSessionRequest(rawRequest);
            request.Parse();
            Assert.Equal("XXXXXX", request.Certificate);
            Assert.Equal("XXXXXX", request.Proof);
            Assert.Equal("0", request.GameId.ToString());
        }
        [Fact]
        public void CreateSession()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:CreateSession>
                            <ns1:certificate>XXXXXX</ns1:certificate>
                            <ns1:proof>XXXXXX</ns1:proof>
                            <ns1:gameid>0</ns1:gameid>
                        </ns1:CreateSession>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new CreateSessionRequest(rawRequest);
            request.Parse();
            Assert.Equal("XXXXXX", request.Certificate);
            Assert.Equal("XXXXXX", request.Proof);
            Assert.Equal("0", request.GameId.ToString());
        }
        [Fact]
        public void SetReportIntention()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:SetReportIntention>
                            <ns1:certificate>XXXXXX</ns1:certificate>
                            <ns1:proof>XXXXXX</ns1:proof>
                            <ns1:csid>0</ns1:csid>
                            <ns1:ccid>0</ns1:ccid>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:authoritative>XXXXXX</ns1:authoritative>
                        </ns1:SetReportIntention>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new SetReportIntentionRequest(rawRequest);
            request.Parse();
            Assert.Equal("XXXXXX", request.Certificate);
            Assert.Equal("XXXXXX", request.Proof);
            Assert.Equal("0", request.CsId.ToString());
            Assert.Equal("0", request.CcId.ToString());
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.Authoritative);
        }
        [Fact]
        public void SubmitReport()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:SubmitReport>
                            <ns1:certificate>XXXXXX</ns1:certificate>
                            <ns1:proof>XXXXXX</ns1:proof>
                            <ns1:csid>0</ns1:csid>
                            <ns1:ccid>0</ns1:ccid>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:authoritative>XXXXXX</ns1:authoritative>
                        </ns1:SubmitReport>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new SubmitReportRequest(rawRequest);
            request.Parse();
            Assert.Equal("XXXXXX", request.Certificate);
            Assert.Equal("XXXXXX", request.Proof);
            Assert.Equal("0", request.CsId.ToString());
            Assert.Equal("0", request.CcId.ToString());
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.Authoritative);
        }
    }
}
