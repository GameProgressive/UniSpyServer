using System;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Enumerate;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using Xunit;

namespace UniSpyServer.Servers.PresenceConnectionManager.Test
{
    public class RequestTest
    {
        [Fact]
        public void LoginAuthtokenTest()
        {
            var request = new LoginRequest(RawRequests.LoginAuthtoken);
            request.Parse();
            Assert.Equal(request.Type, LoginType.AuthToken);
            Assert.Equal(request.UserChallenge, "xxxx");
            Assert.Equal(request.AuthToken, "xxxx");
            Assert.Equal(request.UserID, (uint)0);
            Assert.Equal(request.ProfileID, (uint)0);
            Assert.Equal(request.PartnerID, (uint)0);
            Assert.Equal(request.Response, "xxxxx");
            Assert.Equal(request.Firewall, "1");
            Assert.Equal(request.GamePort, (ushort)0);
            Assert.Equal(request.ProductID, (uint)0);
            Assert.Equal(request.GameName, "gmtest");
            Assert.Equal(request.SDKRevisionType, (SDKRevisionType)3);
            Assert.Equal(request.QuietModeFlags, (QuietModeType)0);
        }
        [Fact]
        public void LoginUniquenickTest()
        {
            var request = new LoginRequest(RawRequests.LoginUniquenick);
            request.Parse();
            Assert.Equal(request.Type, LoginType.UniquenickNamespaceID);
            Assert.Equal(request.UserChallenge, "xxxx");
            Assert.Equal(request.UniqueNick, "xiaojiuwo");
            Assert.Equal(request.NamespaceID, (uint)0);
            Assert.Equal(request.UserID, (uint)0);
            Assert.Equal(request.ProfileID, (uint)0);
            Assert.Equal(request.PartnerID, (uint)0);
            Assert.Equal(request.Response, "xxxxx");
            Assert.Equal(request.Firewall, "1");
            Assert.Equal(request.GamePort, (ushort)0);
            Assert.Equal(request.ProductID, (uint)0);
            Assert.Equal(request.GameName, "gmtest");
            Assert.Equal(request.SDKRevisionType, (SDKRevisionType)3);
            Assert.Equal(request.QuietModeFlags, (QuietModeType)0);
        }
        [Fact]
        public void LoginUserTest()
        {
            var request = new LoginRequest(RawRequests.LoginUser);
            request.Parse();
            Assert.Equal(request.Type, LoginType.NickEmail);
            Assert.Equal(request.UserChallenge, "xxxx");
            Assert.Equal(request.Nick, "xiaojiuwo");
            Assert.Equal(request.Email, "xiaojiuwo@gamespy.com");
            Assert.Equal(request.NamespaceID, (uint)0);
            Assert.Equal(request.UserID, (uint)0);
            Assert.Equal(request.ProfileID, (uint)0);
            Assert.Equal(request.PartnerID, (uint)0);
            Assert.Equal(request.Response, "xxxxx");
            Assert.Equal(request.Firewall, "1");
            Assert.Equal(request.GamePort, (ushort)0);
            Assert.Equal(request.ProductID, (uint)0);
            Assert.Equal(request.GameName, "gmtest");
            Assert.Equal(request.SDKRevisionType, (SDKRevisionType)3);
            Assert.Equal(request.QuietModeFlags, (QuietModeType)0);
        }
    }
}
