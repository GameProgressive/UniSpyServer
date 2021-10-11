using System;
using Xunit;
using WebServer.Entity.Structure.Request.AuthRequest;
namespace UniSpyServer.Servers.WebServer.RequestTest
{
    public class RequestAuthTest
    {
        //
        // These are the SOAP requests for the auth service
        //
        [Fact]
        public void LoginUniqueNick()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:LoginUniqueNick>
                            <ns1:version>1</ns1:version>
                            <ns1:partnercode>95</ns1:partnercode>
                            <ns1:namespaceid>95</ns1:namespaceid>
                            <ns1:uniquenick>crysis2</ns1:uniquenick>
                            <ns1:password>
                                <ns1:Value>XXXXXXXXXXX</ns1:Value>
                            </ns1:password>
                        </ns1:LoginUniqueNick>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new LoginUniqueNickRequest(rawRequest);
            request.Parse();
            Assert.Equal("1", request.Version.ToString());
            Assert.Equal("95", request.PartnerCode.ToString());
            Assert.Equal("95", request.NamespaceId.ToString());
            Assert.Equal("crysis2", request.Uniquenick);
            Assert.Equal("XXXXXXXXXXX", request.Password[0].FieldName);
            Assert.Equal("Value", request.Password[0].FiledType);
        }
    }
}
