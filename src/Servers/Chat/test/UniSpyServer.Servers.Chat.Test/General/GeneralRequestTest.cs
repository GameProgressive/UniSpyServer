using Xunit;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;

namespace UniSpyServer.Servers.Chat.Test.General
{
    public class GeneralRequestTest
    {
        [Fact]
        public void CDKey()
        {
            var request = new CDKeyRequest(GeneralRequests.CDKey);
            request.Parse();
            Assert.Equal("XXXX-XXXX-XXXX-XXXX", request.CDKey);
        }

        [Fact]
        public void Crypt()
        {
            var request = new CryptRequest(GeneralRequests.Crypt);
            request.Parse();
            Assert.Equal("1", request.VersionID);
            Assert.Equal("gmtest", request.GameName);
        }

        [Fact]
        public void GetKey()
        {
            var request = new GetKeyRequest(GeneralRequests.GetKey);
            request.Parse();
            Assert.Equal("SpyGuy", request.NickName);
            Assert.Equal("004", request.Cookie);
            Assert.Equal("0", request.UnkownCmdParam);
            Assert.Equal("b_firewall", request.Keys[0]);
            Assert.Equal("b_profileid", request.Keys[1]);
            Assert.Equal("b_ipaddress", request.Keys[2]);
            Assert.Equal("b_publicip", request.Keys[3]);
            Assert.Equal("b_privateip", request.Keys[4]);
            Assert.Equal("b_authresponse", request.Keys[5]);
            Assert.Equal("b_gamever", request.Keys[6]);
            Assert.Equal("b_val", request.Keys[7]);
        }

        // TODO: add GetUdpRelayRequest

        [Fact]
        public void Invite()
        {
            var request = new InviteRequest(GeneralRequests.Invite);
            request.Parse();
            Assert.Equal("TestRoom", request.ChannelName);
            Assert.Equal("Spyguy", request.UserName);
        }

        [Fact]
        public void ListLimit()
        {
            var request = new ListLimitRequest(GeneralRequests.ListLimit);
            request.Parse();
            Assert.Equal("5", request.MaxNumberOfChannels.ToString());
            Assert.Equal("Test", request.Filter);
        }

        [Fact]
        public void List()
        {
            var request = new ListRequest(GeneralRequests.List);
            request.Parse();
            // TODO: Implement IsSearchingChannel and IsSearchingUser
            //Assert.True(request.IsSearchingChannel);
            //Assert.True(request.IsSearchingUser);
            Assert.Equal("Test", request.Filter);
        }

        [Fact]
        public void LoginPreAuth()
        {
            var request = new LoginPreAuthRequest(GeneralRequests.LoginPreAuth);
            request.Parse();
            Assert.Equal("XXXXXXXXXX", request.AuthToken);
            Assert.Equal("YYYYYYYYYY", request.PartnerChallenge);
        }

        // FIXME:
        /*[Fact]
        public void Login()
        {
            var request = new LoginRequest(GeneralRequests.Login);
            request.Parse();
            Assert.Equal("1", request.NameSpaceID.ToString());
            Assert.Equal("SpyGuy", request.NickName);
            Assert.Equal("spyguy@gamespy.com", request.Email);
            Assert.Equal("SpyGuy", request.UniqueNick);
            Assert.Equal("XXXXXXXXXX", request.PasswordHash);
        }*/

        // TODO: add NamesRequest

        [Fact]
        public void Nick()
        {
            var request = new NickRequest(GeneralRequests.Nick);
            request.Parse();
            Assert.Equal("Wiz", request.NickName);
        }

        // TODO: add PingRequest

        // FIXME:
        /*[Fact]
        public void Pong()
        {
            var request = new PongRequest(GeneralRequests.Pong);
            request.Parse();
            Assert.Equal("test", request.EchoMessage);   
        }*/

        // FIXME:
        /*[Fact]
        public void Quit()
        {
            var request = new QuitRequest(GeneralRequests.Quit);
            request.Parse();
            Assert.Equal("Bye", request.Reason);
        }*/

        [Fact]
        public void RegisterNick()
        {
            var request = new RegisterNickRequest(GeneralRequests.RegisterNick);
            request.Parse();
            Assert.Equal("1", request.NamespaceID);
            Assert.Equal("SpyGuy", request.UniqueNick);
            Assert.Equal("XXXX-XXXX-XXXX-XXXX", request.CDKey);
        }

        [Fact]
        public void SetGroup()
        {
            var request = new SetGroupRequest(GeneralRequests.SetGroup);
            request.Parse();
            Assert.Equal("Test", request.GroupName);
        }

        // TODO: add SetKeyRequest

        // TODO: add UserIPRequest

        [Fact]
        public void User()
        {
            var request = new UserRequest(GeneralRequests.User);
            request.Parse();
            Assert.Equal("pants1", request.UserName);
            Assert.Equal("pants2", request.Name);
            Assert.Equal("127.0.0.1", request.Hostname);
            Assert.Equal("peerchat.gamespy.com", request.ServerName);
        }

        [Fact]
        public void WhoIs()
        {
            var request = new WhoIsRequest(GeneralRequests.WhoIs);
            request.Parse();
            Assert.Equal("SpyGuy", request.NickName);
        }

        [Fact]
        public void Who()
        {
            var request = new WhoRequest(GeneralRequests.Who);
            request.Parse();
            //Assert.Equal("a", request.ChannelName);
            Assert.Equal("SpyGuy", request.NickName);
            // TODO: add WhoRequestType to Assert
        }
    }
}
