using Xunit;
using UniSpyServer.Chat.Entity.Structure.Request.General;

namespace UniSpyServer.Servers.UniSpyServer.Chat.RequestTest
{
    public class GeneralRequestTest
    {
        [Fact]
        public void CDKeyRequestTest()
        {
            var rawRequest = "CDKEY XXXX-XXXX-XXXX-XXXX";
            var request = new CDKeyRequest(rawRequest);
            request.Parse();
            Assert.Equal("XXXX-XXXX-XXXX-XXXX", request.CDKey);
        }

        [Fact]
        public void CryptRequestTest()
        {
            var rawRequest = "CRYPT des 1 gmtest";
            var request = new CryptRequest(rawRequest);
            request.Parse();
            Assert.Equal("1", request.VersionID);
            Assert.Equal("gmtest", request.GameName);
        }

        [Fact]
        public void InviteRequestTest()
        {
            var rawRequest = "INVITE TestRoom Spyguy";
            var request = new InviteRequest(rawRequest);
            request.Parse();
            Assert.Equal("TestRoom", request.ChannelName);
            Assert.Equal("Spyguy", request.UserName);
        }

        [Fact]
        public void ListLimitRequestTest()
        {
            var rawRequest = "LISTLIMIT 5 Test";
            var request = new ListLimitRequest(rawRequest);
            request.Parse();
            Assert.Equal("5", request.MaxNumberOfChannels.ToString());
            Assert.Equal("Test", request.Filter);
        }

        [Fact]
        public void ListRequestTest()
        {
            var rawRequest = "LIST Test";
            var request = new ListRequest(rawRequest);
            request.Parse();
            // TODO: Implement IsSearchingChannel and IsSearchingUser
            //Assert.True(request.IsSearchingChannel);
            //Assert.True(request.IsSearchingUser);
            Assert.Equal("Test", request.Filter);
        }

        [Fact]
        public void LoginPreAuthRequestTest()
        {
            var rawRequest = "LOGINPREAUTH XXXXXXXXXX YYYYYYYYYY";
            var request = new LoginPreAuthRequest(rawRequest);
            request.Parse();
            Assert.Equal("XXXXXXXXXX", request.AuthToken);
            Assert.Equal("YYYYYYYYYY", request.PartnerChallenge);
        }
        // TODO: add GetKeyRequest

        // TODO: add GetUdpRelayRequest

        // FIXME:
        /*[Fact]
        public void LoginRequestTest()
        {
            var rawRequest = "LOGIN 1 SpyGuy spyguy@gamespy.com SpyGuy XXXXXXXXXX";
            var request = new LoginRequest(rawRequest);
            request.Parse();
            Assert.Equal("1", request.NameSpaceID.ToString());
            Assert.Equal("SpyGuy", request.NickName);
            Assert.Equal("spyguy@gamespy.com", request.Email);
            Assert.Equal("SpyGuy", request.UniqueNick);
            Assert.Equal("XXXXXXXXXX", request.PasswordHash);
        }*/

        // TODO: add NamesRequest

        [Fact]
        public void NickRequestTest()
        {
            var rawRequest = "NICK Wiz";
            var request = new NickRequest(rawRequest);
            request.Parse();
            Assert.Equal("Wiz", request.NickName);
        }

        // TODO: add PingRequest

        // FIXME:
        /*[Fact]
        public void PongRequestTest()
        {
            var rawRequest = "PONG test";
            var request = new PongRequest(rawRequest);
            request.Parse();
            Assert.Equal("test", request.EchoMessage);   
        }*/

        // FIXME:
        /*[Fact]
        public void QuitRequestTest()
        {
            var rawRequest = "QUIT Bye";
            var request = new QuitRequest(rawRequest);
            request.Parse();
            Assert.Equal("Bye", request.Reason);
        }*/

        [Fact]
        public void RegisterNickRequestTest()
        {
            var rawRequest = "REGISTERNICK 1 SpyGuy XXXX-XXXX-XXXX-XXXX";
            var request = new RegisterNickRequest(rawRequest);
            request.Parse();
            Assert.Equal("1", request.NamespaceID);
            Assert.Equal("SpyGuy", request.UniqueNick);
            Assert.Equal("XXXX-XXXX-XXXX-XXXX", request.CDKey);
        }

        [Fact]
        public void SetGroupRequestTest()
        {
            var rawRequest = "SETGROUP Test";
            var request = new SetGroupRequest(rawRequest);
            request.Parse();
            Assert.Equal("Test", request.GroupName);
        }

        // TODO: add SetKeyRequest

        // TODO: add UserIPRequest

        [Fact]
        public void UserRequestTest()
        {
            var rawRequest = "USER pants1 127.0.0.1 peerchat.gamespy.com :pants2";
            var request = new UserRequest(rawRequest);
            request.Parse();
            Assert.Equal("pants1", request.UserName);
            Assert.Equal("pants2", request.Name);
            Assert.Equal("127.0.0.1", request.Hostname);
            Assert.Equal("peerchat.gamespy.com", request.ServerName);
        }

        [Fact]
        public void WhoIsRequestTest()
        {
            var rawRequest = "WHOIS SpyGuy";
            var request = new WhoIsRequest(rawRequest);
            request.Parse();
            Assert.Equal("SpyGuy", request.NickName);
        }

        [Fact]
        public void WhoRequestTest()
        {
            var rawRequest = "WHO SpyGuy";
            var request = new WhoRequest(rawRequest);
            request.Parse();
            //Assert.Equal("a", request.ChannelName);
            Assert.Equal("SpyGuy", request.NickName);
            // TODO: add WhoRequestType to Assert
        }
    }
}
