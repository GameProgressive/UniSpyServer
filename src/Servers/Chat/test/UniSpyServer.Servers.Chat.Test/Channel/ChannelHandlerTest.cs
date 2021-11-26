using System;
using System.Linq;
using System.Net;
using UniSpyServer.Servers.Chat.Application;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.Channel;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.Channel;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.General;
using UniSpyServer.Servers.Chat.Handler.CommandSwitcher;
using UniSpyServer.Servers.Chat.Network;
using UniSpyServer.Servers.Chat.Test.General;
using Xunit;

namespace UniSpyServer.Servers.Chat.Test
{
    public class ChannelHandlerTest
    {
        public ChannelHandlerTest()
        {
        }
        [Fact]
        public void JoinTest()
        {
            var factory = new ServerFactory();
            factory.Start();
            var session1 = new Session(ServerFactory.Server);
            var session2 = new Session(ServerFactory.Server);

            var user1Req = new UserRequest("USER spyguy1 127.0.0.1 peerchat.unispy.org :spyguy1");
            var user2Req = new UserRequest("USER spyguy1 127.0.0.1 peerchat.unispy.org :spyguy1");
            var nick1Req = new NickRequest("NICK spyguy1");
            var nick2Req = new NickRequest("NICK spyguy2");
            var joinReq = new JoinRequest("JOIN #GSP!room!testr");

            var user1Handler = new UserHandler(session1, user1Req);
            var user2Handler = new UserHandler(session2, user2Req);
            var nick1Handler = new NickHandler(session1, nick1Req);
            var nick2Handler = new NickHandler(session2, nick2Req);
            var join1Handler = new JoinHandler(session1, joinReq);
            var join2Handler = new JoinHandler(session2, joinReq);
            user1Handler.Handle();
            user2Handler.Handle();
            nick1Handler.Handle();
            nick2Handler.Handle();
            join1Handler.Handle();
            join2Handler.Handle();
            Assert.Equal(1, session1.UserInfo.JoinedChannels.Count);
            Assert.Equal(1, session2.UserInfo.JoinedChannels.Count);
            Assert.Equal("#GSP!room!testr", session1.UserInfo.JoinedChannels.First().Property.ChannelName);
            Assert.Equal("#GSP!room!testr", session2.UserInfo.JoinedChannels.First().Property.ChannelName);
            Assert.True(session1.UserInfo.IsJoinedChannel("#GSP!room!testr"));
            Assert.True(session2.UserInfo.IsJoinedChannel("#GSP!room!testr"));

        }
    }
}