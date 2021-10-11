using System;
using Xunit;
using WebServer.Entity.Structure.Request.AuthRequest;
namespace UniSpyServer.Servers.WebServer.RequestTest
{
    public class RequestAuthTest
    {
        //
        // These are the SOAP requests of AUTH
        // Endpoint: {FQDN}/AuthService/
        //
        [Fact]
        public void LoginProfile()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:LoginProfile>
                            <ns1:version>1</ns1:version>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:partnercode>0</ns1:partnercode>
                            <ns1:namespaceid>0</ns1:namespaceid>
                            <ns1:email>spyguy@unispy.org</ns1:email>
                            <ns1:uniquenick>spyguy</ns1:uniquenick>
                            <ns1:cdkey>XXXXXXXXXXX</ns1:cdkey>
                            <ns1:password>
                                <ns1:Value>XXXXXXXXXXX</ns1:Value>
                            </ns1:password>
                        </ns1:LoginProfile>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new LoginProfileRequest(rawRequest);
            request.Parse();
            Assert.Equal("1", request.Version.ToString());
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.PartnerCode.ToString());
            Assert.Equal("0", request.NamespaceId.ToString());
            Assert.Equal("spyguy@unispy.org", request.Email);
            Assert.Equal("spyguy", request.Uniquenick);
            Assert.Equal("XXXXXXXXXXX", request.CDKey);
            Assert.Equal("XXXXXXXXXXX", request.Password[0].FieldName);
            Assert.Equal("Value", request.Password[0].FiledType);
        }
        [Fact]
        public void LoginPs3Cert()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:LoginPs3Cert>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:partnercode>0</ns1:partnercode>
                            <ns1:ps3cert>0</ns1:ps3cert>
                        </ns1:LoginPs3Cert>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new LoginPs3CertRequest(rawRequest);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.PartnerCode.ToString());
            Assert.Equal("0", request.PS3cert.ToString());
        }
        [Fact]
        public void LoginRemoteAuth()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:LoginRemoteAuth>
                            <ns1:version>1</ns1:version>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:partnercode>0</ns1:partnercode>
                            <ns1:namespaceid>0</ns1:namespaceid>
                            <ns1:authtoken>XXXXXXXXXXX</ns1:authtoken>
                            <ns1:challenge>XXXXXXXXXXX</ns1:challenge>
                        </ns1:LoginRemoteAuth>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new LoginRemoteAuthRequest(rawRequest);
            request.Parse();
            Assert.Equal("1", request.Version.ToString());
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.PartnerCode.ToString());
            Assert.Equal("0", request.NamespaceId.ToString());
            Assert.Equal("XXXXXXXXXXX", request.AuthToken);
            Assert.Equal("XXXXXXXXXXX", request.Challenge);
        }
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
                            <ns1:partnercode>0</ns1:partnercode>
                            <ns1:namespaceid>0</ns1:namespaceid>
                            <ns1:uniquenick>spyguy</ns1:uniquenick>
                            <ns1:password>
                                <ns1:Value>XXXXXXXXXXX</ns1:Value>
                            </ns1:password>
                        </ns1:LoginUniqueNick>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new LoginUniqueNickRequest(rawRequest);
            request.Parse();
            Assert.Equal("1", request.Version.ToString());
            Assert.Equal("0", request.PartnerCode.ToString());
            Assert.Equal("0", request.NamespaceId.ToString());
            Assert.Equal("spyguy", request.Uniquenick);
            Assert.Equal("XXXXXXXXXXX", request.Password[0].FieldName);
            Assert.Equal("Value", request.Password[0].FiledType);
        }
    }
}
