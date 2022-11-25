using System;
using System.Numerics;
using UniSpyServer.Servers.WebServer.Entity.Structure;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request;
using UniSpyServer.Servers.WebServer.Module.Direct2Game.Entity.Structure.Response;
using UniSpyServer.UniSpyLib.Extensions;
using Xunit;

namespace UniSpyServer.Servers.WebServer.Test.Auth
{
    public class RequestsTest
    {
        //
        // These are the SOAP requests of AUTH
        // Endpoint: {FQDN}/AuthService/
        //
        [Fact]
        public void LoginProfile()
        {
            var request = new LoginProfileWithGameIdRequest(RawRequests.LoginProfile);
            request.Parse();
            Assert.Equal("1", request.Version.ToString());
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.PartnerCode.ToString());
            Assert.Equal("0", request.NamespaceId.ToString());
            Assert.Equal("spyguy@unispy.org", request.Email);
            Assert.Equal("spyguy", request.Uniquenick);
            Assert.Equal("XXXXXXXXXXX", request.CDKey);
            Assert.Equal("XXXXXXXXXXX", request.Password);
        }
        [Fact]
        public void LoginPs3Cert()
        {
            var request = new LoginPs3CertWithGameIdRequest(RawRequests.LoginPs3Cert);
            request.Parse();
            Assert.Equal(0, request.GameId);
            Assert.Equal(1, request.PartnerCode);
            Assert.Equal("0001", request.PS3cert);
        }
        [Fact]
        public void LoginRemoteAuth()
        {
            var request = new LoginRemoteAuthWithGameIdRequest(RawRequests.LoginRemoteAuth);
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
            var request = new LoginUniqueNickRequest(RawRequests.LoginUniqueNick);
            request.Parse();
            Assert.Equal("1", request.Version.ToString());
            Assert.Equal("0", request.PartnerCode.ToString());
            Assert.Equal("0", request.NamespaceId.ToString());
            Assert.Equal("spyguy", request.Uniquenick);
            Assert.Equal("XXXXXXXXXXX", request.Password);
        }

        [Fact]
        public void CustomRSATest()
        {
            BigInteger publicExponent = BigInteger.Parse("010001", System.Globalization.NumberStyles.AllowHexSpecifier);
            BigInteger privateExponent = BigInteger.Parse("00af12efb486a5f594f4b86d153cef694fba59bde5005411e271ad9e53ae41bd3183b3b06459de85907bfdcee256180bd450f7f547dd1c81f57e14b477a48cef415f957de5ea723a0050be386fd2c1369761340f23ed43aa4299926107e3c56845ea32685a7ced12a32bfd6b6a2aefe8b8b9fdf0893f486342f36fd6000d691ee1", System.Globalization.NumberStyles.AllowHexSpecifier);

            BigInteger Modulo = BigInteger.Parse("00e2201247fcb3ef29e45e842eee4a1b072ae59c115de6f4e0bb857b4e9282b2ee2b6ce1aef46a5eea7ad7bd0a5a4a969a186e2dd1e379e4a27a0e2120f49da702d4a3b892f5c776fee869d218f145b6e32f32b71063a0222addc256e8fdc977b2324a71370777295d45240f4f5fdf7cd7ab9c2393fdce0781c5118b9e1e905537", System.Globalization.NumberStyles.AllowHexSpecifier);

            var data = new BigInteger(123);

            var enc = BigInteger.ModPow(data, privateExponent, Modulo);
            var dec = BigInteger.ModPow(enc, publicExponent, Modulo);

            var signature = enc.ToString("x");
            var bytes = enc.ToByteArray(isBigEndian: true);
            var gamespyFormat = BitConverter.ToString(bytes).Replace("-", string.Empty);

            var sigbytes = "0001FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF003020300C06082A864886F70D020505000410".FromHexStringToBytes();
        }


        [Fact]
        public void XmlTest()
        {
            // XNamespace SoapEnvelopNamespace = "http://schemas.xmlsoap.org/soap/envelope/";

            // var env = new XElement(SoapEnvelopNamespace + "Envelope",
            //                          new XAttribute(XNamespace.Xmlns + "SOAP-ENV", SoapEnvelopNamespace),
            //                          new XAttribute(XNamespace.Xmlns + "SOAP-ENC", "http://schemas.xmlsoap.org/soap/encoding/"),
            //                          new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
            //                          new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"));
            // XNamespace SakeNamespace = "http://gamespy.net/sake";
            // var attri = new XAttribute(XNamespace.Xmlns + "ns1", SakeNamespace);
            // env.Add(attri);
            // env.SetAttributeValue(XNamespace.Xmlns + "ns1", SakeNamespace);
            // var soapEnvelop = new XElement(SoapXElement.Direct2GameSoapHeader);
            // soapEnvelop.Add(new XElement(SoapXElement.SoapEnvelopNamespace + "Body"));
            // var nn1 = soapEnvelop.NextNode;

            // var _soapBody = new XElement(SoapXElement.SoapEnvelopNamespace + "Body");
            // _soapBody.Add(new XElement(SoapXElement.Direct2GameNamespace + "GetStoreAvailabilityResult"));

            // var e = _soapBody.Elements().First();
            // e.Add(new XElement(SoapXElement.Direct2GameNamespace + "status"));
            // e.Elements().First().Add(new XElement(SoapXElement.Direct2GameNamespace + "code", 0));
            // e.Add(new XElement(SoapXElement.Direct2GameNamespace + "storestatusid", 1));

            // _soapBody.Add(e);
            // soapEnvelop.Add(e);
            // soapEnvelop.DescendantNodes();
            // soapEnvelop.Descendants();
            // soapEnvelop.Nodes();
            // var nn = soapEnvelop.NextNode;
            // var auth = new AuthSoapEnvelope();
            // auth.Add("heello", "hello");
            // auth.Add("bitch", "bitch");
            // // var nn2 = soapEnvelop.FirstNode.NextNode.FirstNode;
            // // Given
            // var authEle = new AuthXElement("certificate");
            // authEle.Add("authtoken", "XXXXXXXXXXX");
            // authEle.Add("challenge", "XXXXXXXXXXX");
            // authEle.Add("email", "xiaojiuwo@gamspy.com");
            // authEle.Add("password", "XXXXXXXXXXX");
            // authEle.Add("partnercode", "0");
            // authEle.Add("uniquenick", "xiaojiuwo");
            // auth.Add("element", authEle.InnerElement);

            // When
            var _content = new AuthSoapEnvelope();
            _content.Add("responseCode", 0);
            _content.Add("certificate");
            _content.Add("length", 0);
            _content.Add("version", 2);
            _content.Add("partnercode", 2);
            _content.Add("namespaceid", 4);
            _content.Add("userid", 1);
            _content.Add("profileid", 1);
            _content.Add("expretime", ClientInfo.ExpireTime);
            _content.Add("uniqueid", "xiaojiuwo");
            _content.ChangeToElement("Body");
            _content.Add("values");
            _content.Add("value1", 1);
            _content.Add("value2", 2);
            _content.Add("value3", 3);
            // Then
        }

        [Fact(Skip = "not implemented")]
        public void TestName()
        {
            // Given
            var resp = new GetPurchaseHistoryResponse(null, null);
            resp.Build();
            // When

            // Then
        }
    }
}
