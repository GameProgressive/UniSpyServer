using System.Linq;
using UniSpyServer.Servers.Chat.Application;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.Channel;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.General;
using UniSpyServer.Servers.Chat.Network;
using Xunit;

namespace UniSpyServer.Servers.Chat.Test
{
    public class ChannelHandlerTest
    {
        private ServerFactory _serverFactory;
        public ChannelHandlerTest()
        {
            _serverFactory = new ServerFactory();
            _serverFactory.Start();
        }
        [Fact]
        public void JoinTest()
        {
            var session1 = SingleJoinTest("unispy1", "unispy1", "#GSP!room!test");
            var session2 = SingleJoinTest("unispy2", "unispy2", "#GSP!room!test");
        }

        public Session SingleJoinTest(string userName = "unispy", string nickName = "unispy", string channelName = "#GSP!room!test")
        {
            var session = new Session(ServerFactory.Server);

            var userReq = new UserRequest($"USER {userName} 127.0.0.1 peerchat.unispy.org :{userName}");
            var nickReq = new NickRequest($"NICK {nickName}");
            var joinReq = new JoinRequest($"JOIN {channelName}");

            var userHandler = new UserHandler(session, userReq);
            var nickHandler = new NickHandler(session, nickReq);
            var joinHandler = new JoinHandler(session, joinReq);
            userHandler.Handle();
            nickHandler.Handle();
            // we know the endpoint object is not set, so System.NullReferenceException will be thrown
            Assert.Throws<System.NullReferenceException>(() => joinHandler.Handle());
            Assert.Equal(1, session.UserInfo.JoinedChannels.Count);
            Assert.Equal(channelName, session.UserInfo.JoinedChannels.First().Property.ChannelName);
            Assert.True(session.UserInfo.IsJoinedChannel(channelName));
            return session;
        }
    }
}