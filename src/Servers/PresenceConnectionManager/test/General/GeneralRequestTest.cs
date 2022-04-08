using UniSpyServer.Servers.PresenceConnectionManager.Entity.Enumerate;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using Xunit;

namespace UniSpyServer.Servers.PresenceConnectionManager.Test
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
            Assert.Equal((int)0, request.UserID);
            Assert.Equal((int)0, request.ProfileId);
            Assert.Equal((int)0, request.PartnerID);
            Assert.Equal("xxxxx", request.Response);
            Assert.Equal("1", request.Firewall);
            Assert.Equal(request.GamePort, (ushort)0);
            Assert.Equal(request.ProductID, (int)0);
            Assert.Equal("gmtest", request.GameName);
            Assert.Equal((SdkRevisionType)3, request.SdkRevisionType);
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
            Assert.Equal((int)0, request.NamespaceID);
            Assert.Equal((int)0, request.UserID);
            Assert.Equal((int)0, request.ProfileId);
            Assert.Equal((int)0, request.PartnerID);
            Assert.Equal("xxxxx", request.Response);
            Assert.Equal("1", request.Firewall);
            Assert.Equal((ushort)0, request.GamePort);
            Assert.Equal((int)0, request.ProductID);
            Assert.Equal("gmtest", request.GameName);
            Assert.Equal((SdkRevisionType)3, request.SdkRevisionType);
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
            Assert.Equal((int)0, request.NamespaceID);
            Assert.Equal((int)0, request.UserID);
            Assert.Equal((int)0, request.ProfileId);
            Assert.Equal((int)0, request.PartnerID);
            Assert.Equal("xxxxx", request.Response);
            Assert.Equal("1", request.Firewall);
            Assert.Equal((ushort)0, request.GamePort);
            Assert.Equal((int)0, request.ProductID);
            Assert.Equal("gmtest", request.GameName);
            Assert.Equal((SdkRevisionType)3, request.SdkRevisionType);
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
            Assert.Equal((int)0, request.ProductID);
            Assert.Equal("gmtest", request.GameName);
            Assert.Equal((int)0, request.NamespaceID);
            Assert.Equal("spyguy", request.Uniquenick);
            Assert.Equal("xxxx", request.CDKeyEnc);
        }*/
    }
}
