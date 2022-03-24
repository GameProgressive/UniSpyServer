using System.Numerics;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request;
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
            var request = new LoginProfileRequest(RawRequests.LoginProfile);
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
            var request = new LoginPs3CertRequest(RawRequests.LoginPs3Cert);
            request.Parse();
            Assert.Equal(0, request.GameId);
            Assert.Equal(0, request.PartnerCode);
            Assert.Equal("0001", request.PS3cert);
        }
        [Fact]
        public void LoginRemoteAuth()
        {
            var request = new LoginRemoteAuthRequest(RawRequests.LoginRemoteAuth);
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
            BigInteger pubExpo = BigInteger.Parse("010001", System.Globalization.NumberStyles.AllowHexSpecifier);
            BigInteger PriExpo = BigInteger.Parse("00af12efb486a5f594f4b86d153cef694fba59bde5005411e271ad9e53ae41bd3183b3b06459de85907bfdcee256180bd450f7f547dd1c81f57e14b477a48cef415f957de5ea723a0050be386fd2c1369761340f23ed43aa4299926107e3c56845ea32685a7ced12a32bfd6b6a2aefe8b8b9fdf0893f486342f36fd6000d691ee1", System.Globalization.NumberStyles.AllowHexSpecifier);

            BigInteger Modulo = BigInteger.Parse("00e2201247fcb3ef29e45e842eee4a1b072ae59c115de6f4e0bb857b4e9282b2ee2b6ce1aef46a5eea7ad7bd0a5a4a969a186e2dd1e379e4a27a0e2120f49da702d4a3b892f5c776fee869d218f145b6e32f32b71063a0222addc256e8fdc977b2324a71370777295d45240f4f5fdf7cd7ab9c2393fdce0781c5118b9e1e905537", System.Globalization.NumberStyles.AllowHexSpecifier);


            var data = new BigInteger(123);

            var enc = BigInteger.ModPow(data, PriExpo, Modulo);
            var dec = BigInteger.ModPow(enc, pubExpo, Modulo);

        }
    }
}
