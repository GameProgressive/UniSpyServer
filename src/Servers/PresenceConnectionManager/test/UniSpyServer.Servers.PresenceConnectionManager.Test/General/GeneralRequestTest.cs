using UniSpyServer.Servers.PresenceConnectionManager.Entity.Enumerate;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request.General;
using Xunit;

namespace UniSpyServer.Servers.PresenceConnectionManager.Test.General
{
    public class GeneralRequestTest
    {
        //TODO: no keep alive request
        /*[Fact]
        public void KeepAlive()
        {
            var request = new KeepAliveRequest(GeneralRequests.KeepAlive);
            request.Parse();
            Assert.Equal("xxxx", "xxxx");
        }*/

        [Fact]
        public void LoginAuthtoken()
        {
            var request = new LoginRequest(GeneralRequests.LoginAuthtoken);
            request.Parse();
            Assert.Equal(LoginType.AuthToken, request.Type);
            Assert.Equal("xxxx", request.UserChallenge);
            Assert.Equal("xxxx", request.AuthToken);
            Assert.Equal((uint)0, request.UserID);
            Assert.Equal((uint)0, request.ProfileID);
            Assert.Equal((uint)0, request.PartnerID);
            Assert.Equal("xxxxx", request.Response);
            Assert.Equal("1", request.Firewall);
            Assert.Equal(request.GamePort, (ushort)0);
            Assert.Equal(request.ProductID, (uint)0);
            Assert.Equal("gmtest", request.GameName);
            Assert.Equal((SDKRevisionType)3, request.SDKRevisionType);
            Assert.Equal((QuietModeType)0, request.QuietModeFlags);
        }

        [Fact]
        public void LoginUniquenick()
        {
            var request = new LoginRequest(GeneralRequests.LoginUniquenick);
            request.Parse();
            Assert.Equal(LoginType.UniquenickNamespaceID, request.Type);
            Assert.Equal("xxxx", request.UserChallenge);
            Assert.Equal("spyguy", request.UniqueNick);
            Assert.Equal((uint)0, request.NamespaceID);
            Assert.Equal((uint)0, request.UserID);
            Assert.Equal((uint)0, request.ProfileID);
            Assert.Equal((uint)0, request.PartnerID);
            Assert.Equal("xxxxx", request.Response);
            Assert.Equal("1", request.Firewall);
            Assert.Equal((ushort)0, request.GamePort);
            Assert.Equal((uint)0, request.ProductID);
            Assert.Equal("gmtest", request.GameName);
            Assert.Equal((SDKRevisionType)3, request.SDKRevisionType);
            Assert.Equal((QuietModeType)0, request.QuietModeFlags);
        }

        [Fact]
        public void LoginUser()
        {
            var request = new LoginRequest(GeneralRequests.LoginUser);
            request.Parse();
            Assert.Equal(LoginType.NickEmail, request.Type);
            Assert.Equal("xxxx", request.UserChallenge);
            Assert.Equal("spyguy", request.Nick);
            Assert.Equal("spyguy@unispy.org", request.Email);
            Assert.Equal((uint)0, request.NamespaceID);
            Assert.Equal((uint)0, request.UserID);
            Assert.Equal((uint)0, request.ProfileID);
            Assert.Equal((uint)0, request.PartnerID);
            Assert.Equal("xxxxx", request.Response);
            Assert.Equal("1", request.Firewall);
            Assert.Equal((ushort)0, request.GamePort);
            Assert.Equal((uint)0, request.ProductID);
            Assert.Equal("gmtest", request.GameName);
            Assert.Equal((SDKRevisionType)3, request.SDKRevisionType);
            Assert.Equal((QuietModeType)0, request.QuietModeFlags);
        }

        //TODO: no logout request
        /*[Fact]
        public void Logout()
        {
            var request = new LogoutRequest(GeneralRequests.Logout);
            request.Parse();
            Assert.Equal("xxxx", "xxxx");
        }*/

        //TODO: passwordenc doesn't work
        /*[Fact]
        public void NewUser()
        {
            var request = new NewUserRequest(GeneralRequests.NewUser);
            request.Parse();
            Assert.Equal("spyguy@unispy.org", request.Email);
            Assert.Equal("spyguy", request.Nick);
            Assert.Equal("0000", request.Password);
            Assert.Equal((uint)0, request.ProductID);
            Assert.Equal("gmtest", request.GameName);
            Assert.Equal((uint)0, request.NamespaceID);
            Assert.Equal("spyguy", request.Uniquenick);
            Assert.Equal("xxxx", request.CDKeyEnc);
        }*/
    }
}
