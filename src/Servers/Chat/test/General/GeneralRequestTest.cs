using Xunit;
using UniSpy.Server.Chat.Contract.Request.General;

namespace UniSpy.Server.Chat.Test.General
{
    public class GeneralRequestTest
    {
        [Fact]
        public void CDKey()
        {
            var request = new CdKeyRequest(GeneralRequests.CDKey);
            request.Parse();
            Assert.Equal("XXXX-XXXX-XXXX-XXXX", request.CdKey);
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
            Assert.Equal("spyguy", request.NickName);
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

        [Fact(Skip = "No reqeust")]
        public void GetUdpRelay()
        {
            var request = new GetUdpRelayRequest(GeneralRequests.GetUdpRelay);
            request.Parse();
        }

        [Fact]
        public void Invite()
        {
            var request = new InviteRequest(GeneralRequests.Invite);
            request.Parse();
            Assert.Equal("test", request.ChannelName);
            Assert.Equal("spyguy", request.UserName);
        }

        [Fact]
        public void ListLimit()
        {
            var request = new ListLimitRequest(GeneralRequests.ListLimit);
            request.Parse();
            Assert.Equal("5", request.MaxNumberOfChannels.ToString());
            Assert.Equal("test", request.Filter);
        }

        [Fact]
        public void List()
        {
            var request = new ListRequest(GeneralRequests.List);
            request.Parse();
            Assert.Equal("test", request.Filter);
        }

        [Fact]
        public void LoginPreAuth()
        {
            var request = new LoginPreAuth(GeneralRequests.LoginPreAuth);
            request.Parse();
            Assert.Equal("xxxxx", request.AuthToken);
            Assert.Equal("yyyyy", request.PartnerChallenge);
        }

        [Fact]
        public void LoginNickAndEmail()
        {
            var request = new LoginRequest(GeneralRequests.LoginNickAndEmail);
            request.Parse();
            Assert.Equal(LoginReqeustType.NickAndEmailLogin, request.ReqeustType);
            Assert.Equal("0", request.NamespaceID.ToString());
            Assert.Equal("spyguy", request.NickName);
            Assert.Equal("xxxxx", request.PasswordHash);
            Assert.Equal("spyguy@unispy.org", request.Email);
        }

        [Fact]
        public void LoginUniqueNick()
        {
            var request = new LoginRequest(GeneralRequests.LoginUniqueNick);
            request.Parse();
            Assert.Equal(LoginReqeustType.UniqueNickLogin, request.ReqeustType);
            Assert.Equal("0", request.NamespaceID.ToString());
            Assert.Equal("spyguy", request.UniqueNick);
            Assert.Equal("xxxxx", request.PasswordHash);
        }

        [Fact(Skip = "No reqeust")]
        public void Names()
        {
            var request = new NamesRequest(GeneralRequests.Names);
            request.Parse();
        }

        [Fact]
        public void Nick()
        {
            var request = new NickRequest(GeneralRequests.Nick);
            request.Parse();
            Assert.Equal("spyguy", request.NickName);
        }

        [Fact(Skip = "No reqeust")]
        public void Ping()
        {
            var request = new PingRequest(GeneralRequests.Ping);
            request.Parse();
        }

        [Fact]
        public void Pong()
        {
            var request = new PongRequest(GeneralRequests.Pong);
            request.Parse();
            Assert.Equal("Pong!", request.EchoMessage);
        }

        [Fact]
        public void Quit()
        {
            var request = new QuitRequest(GeneralRequests.Quit);
            request.Parse();
            Assert.Equal("Later!", request.Reason);
        }

        [Fact]
        public void RegisterNick()
        {
            var request = new RegisterNickRequest(GeneralRequests.RegisterNick);
            request.Parse();
            Assert.Equal("0", request.NamespaceID);
            Assert.Equal("spyguy", request.UniqueNick);
            Assert.Equal("XXXX-XXXX-XXXX-XXXX", request.CDKey);
        }

        [Fact]
        public void SetGroup()
        {
            var request = new SetGroupRequest(GeneralRequests.SetGroup);
            request.Parse();
            Assert.Equal("test", request.GroupName);
        }

        [Fact(Skip = "TODO: add test")]
        public void SetKey()
        {
            var request = new SetKeyRequest(GeneralRequests.SetKey);
            request.Parse();
        }

        [Fact(Skip = "No reqeust")]
        public void UserIP()
        {
            var request = new UserIPRequest(GeneralRequests.UserIP);
            request.Parse();
        }

        [Fact]
        public void User()
        {
            var request = new UserRequest(GeneralRequests.User);
            request.Parse();
            Assert.Equal("spyguy", request.UserName);
            Assert.Equal("127.0.0.1", request.Hostname);
            Assert.Equal("peerchat.unispy.org", request.ServerName);
            Assert.Equal("spyguy2", request.Name);
        }

        [Fact]
        public void WhoIs()
        {
            var request = new WhoIsRequest(GeneralRequests.WhoIs);
            request.Parse();
            Assert.Equal("spyguy", request.NickName);
        }

        [Fact]
        public void WhoChannelUsersInfo()
        {
            var request = new WhoRequest(GeneralRequests.WhoChannelUsersInfo);
            request.Parse();
            Assert.Equal(WhoRequestType.GetChannelUsersInfo, request.RequestType);
            Assert.Equal("#room", request.ChannelName);
        }

        [Fact]
        public void WhoUserInfo()
        {
            var request = new WhoRequest(GeneralRequests.WhoUserInfo);
            request.Parse();
            Assert.Equal(WhoRequestType.GetUserInfo, request.RequestType);
            Assert.Equal("spyguy", request.NickName);
        }
    }
}
