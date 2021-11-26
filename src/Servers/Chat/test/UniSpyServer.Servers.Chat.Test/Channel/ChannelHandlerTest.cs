using System;
using System.Net;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.General;
using UniSpyServer.Servers.Chat.Network;
using UniSpyServer.Servers.Chat.Test.General;
using Xunit;

namespace UniSpyServer.Servers.Chat.Test
{
    public class TestServer : Server
    {
        public TestServer() : base(new Guid(), new IPEndPoint(IPAddress.Any, 6667))
        {
        }
        public void OnConnected(Session session) => base.OnConnected(session);
        public void OnDisconnected(Session session) => base.OnDisconnected(session);
        public Session CreateTestSession() => (Session)base.CreateSession();
    }

    public class ChannelHandlerTest
    {
        public ChannelHandlerTest()
        {
        }
        [Fact]
        public void JoinTest()
        {
            var server = new TestServer();
            var session = server.CreateTestSession();
            server.OnConnected(session);
            var userReq = new UserRequest(GeneralRequests.User);
            var userHandler = new UserHandler(session, userReq);
            userHandler.Handle();
            Assert.Equal(1, server.SessionManager.SessionPool.Count);

        }
    }
}