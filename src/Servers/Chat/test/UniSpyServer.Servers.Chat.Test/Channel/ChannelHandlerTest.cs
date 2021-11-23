using NetCoreServer;
using UniSpyServer.Servers.Chat.Application;
using UniSpyServer.Servers.Chat.Test.Channel;
using UniSpyServer.Servers.Chat.Test.General;
using Xunit;

namespace UniSpyServer.Servers.Chat.Test
{
    public class ChannelHandlerTest
    {
        // private static ServerFactory _serverFactory;

        public ChannelHandlerTest()
        {
            // _serverFactory = new ServerFactory();
            // _serverFactory.Start();
        }
        [Fact]
        public void JoinTest()
        {
            var server = new ServerFactory();
            server.Start();
            var client1 = new TcpClient("127.0.0.1", 6667);
            client1.Connect();
            client1.SendAsync(GeneralRequests.User);
            client1.SendAsync(GeneralRequests.Nick);
            client1.SendAsync(ChannelRequests.Join);
            Assert.Equal(1, ServerFactory.Server.SessionManager.SessionPool.Count);
        }
    }
}