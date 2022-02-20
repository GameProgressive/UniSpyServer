using UniSpyServer.Servers.Chat.Entity.Structure;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.Channel;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.General;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.Message;
using Xunit;

namespace UniSpyServer.Servers.Chat.Test.Channel
{
    public class ChannelHandlerTest
    {
        public ChannelHandlerTest()
        {
        }
        [Fact]
        public void JoinHandleTest()
        {
            var session1 = SingleJoinTest("unispy1", "unispy1", "#GSP!room!test");
            var session2 = SingleJoinTest("unispy2", "unispy2", "#GSP!room!test");
        }
        [Fact]
        public void ChannelMsgTest()
        {
            var session1 = SingleJoinTest("unispy1", "unispy1", "#GSP!room!test");
            var session2 = SingleJoinTest("unispy2", "unispy2", "#GSP!room!test");
            var privMsgReq = new PrivateMsgRequest("PRIVMSG #GSP!room!test :hello this is a test.");
            var privMsgHandler = new PrivateMsgHandler(session1, privMsgReq);
            privMsgHandler.Handle();
        }
        public Client SingleLoginTest(string userName = "unispy", string nickName = "unispy")
        {
            var session = new Session(ServerFactory.Server);
            var userReq = new UserRequest($"USER {userName} 127.0.0.1 peerchat.unispy.org :{userName}");
            var nickReq = new NickRequest($"NICK {nickName}");
            var userHandler = new UserHandler(session, userReq);
            var nickHandler = new NickHandler(session, nickReq);
            userHandler.Handle();
            nickHandler.Handle();
            return session;
        }
        public Session SingleJoinTest(string userName = "unispy", string nickName = "unispy", string channelName = "#GSP!room!test")
        {
            var session = SingleLoginTest(userName, nickName);
            var joinReq = new JoinRequest($"JOIN {channelName}");
            var joinHandler = new JoinHandler(session, joinReq);
            // we know the endpoint object is not set, so System.NullReferenceException will be thrown
            joinHandler.Handle();
            Assert.Single(session.UserInfo.JoinedChannels);
            Assert.True(session.UserInfo.JoinedChannels.ContainsKey(channelName));
            Assert.True(session.UserInfo.IsJoinedChannel(channelName));
            return session;
        }
        [Fact]
        public void SetCKeyTest()
        {
            var session = SingleJoinTest("spyguy", "spyguy");
            var request = new SetCKeyRequest(ChannelRequests.SetCKey);
            var handler = new SetCKeyHandler(session, request);
            handler.Handle();
        }
        [Fact]
        public void ModeTest()
        {
            var session = SingleJoinTest("spyguy", "spyguy", "#GSP!gmtest!MlNK4q4l1M");
            var request = new ModeRequest("MODE #GSP!gmtest!MlNK4q4l1M -i-p-s+m-n+t+l+e 2");
            var handler = new ModeHandler(session, request);
            handler.Handle();
        }
    }
}