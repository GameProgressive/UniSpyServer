using System.Net;
using Moq;
using UniSpyServer.Servers.Chat.Entity.Structure;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.Channel;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.General;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.Message;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using Xunit;

namespace UniSpyServer.Servers.Chat.Test.Channel
{
    public class ChannelHandlerTest
    {
        private Client _client;
        public ChannelHandlerTest()
        {
            var serverMock = new Mock<IServer>();
            serverMock.Setup(s => s.ServerID).Returns(new System.Guid());
            serverMock.Setup(s => s.ServerName).Returns("Chat");
            serverMock.Setup(s => s.Endpoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var connectionMock = new Mock<ITcpConnection>();
            connectionMock.Setup(s => s.RemoteIPEndPoint).Returns(serverMock.Object.Endpoint);
            connectionMock.Setup(s => s.Server).Returns(serverMock.Object);
            connectionMock.Setup(s=>s.ConnectionType).Returns(NetworkConnectionType.Test);
            _client = new Client(connectionMock.Object);
        }
        [Fact]
        public void JoinHandleTest()
        {
            var connection1 = SingleJoinTest("unispy1", "unispy1", "#GSP!room!test");
            var connection2 = SingleJoinTest("unispy2", "unispy2", "#GSP!room!test");
        }
        [Fact]
        public void ChannelMsgTest()
        {
            var connection1 = SingleJoinTest("unispy1", "unispy1", "#GSP!room!test");
            var connection2 = SingleJoinTest("unispy2", "unispy2", "#GSP!room!test");
            var privMsgReq = new PrivateMsgRequest("PRIVMSG #GSP!room!test :hello this is a test.");
            var privMsgHandler = new PrivateMsgHandler(connection1, privMsgReq);
            privMsgHandler.Handle();
        }
        public IClient SingleLoginTest(string userName = "unispy", string nickName = "unispy")
        {
            var userReq = new UserRequest($"USER {userName} 127.0.0.1 peerchat.unispy.org :{userName}");
            var nickReq = new NickRequest($"NICK {nickName}");
            var userHandler = new UserHandler(_client, userReq);
            var nickHandler = new NickHandler(_client, nickReq);
            userHandler.Handle();
            nickHandler.Handle();
            return _client;
        }
        public IClient SingleJoinTest(string userName = "unispy", string nickName = "unispy", string channelName = "#GSP!room!test")
        {
            var connection = SingleLoginTest(userName, nickName);
            var joinReq = new JoinRequest($"JOIN {channelName}");
            var joinHandler = new JoinHandler(connection, joinReq);
            // we know the endpoint object is not set, so System.NullReferenceException will be thrown
            joinHandler.Handle();
            Assert.Single(_client.Info.JoinedChannels);
            Assert.True(_client.Info.JoinedChannels.ContainsKey(channelName));
            Assert.True(_client.Info.IsJoinedChannel(channelName));
            return connection;
        }
        [Fact]
        public void SetCKeyTest()
        {
            var connection = SingleJoinTest("spyguy", "spyguy");
            var request = new SetCKeyRequest(ChannelRequests.SetCKey);
            var handler = new SetCKeyHandler(connection, request);
            handler.Handle();
        }
        [Fact]
        public void ModeTest()
        {
            var connection = SingleJoinTest("spyguy", "spyguy", "#GSP!gmtest!MlNK4q4l1M");
            var request = new ModeRequest("MODE #GSP!gmtest!MlNK4q4l1M -i-p-s+m-n+t+l+e 2");
            var handler = new ModeHandler(connection, request);
            handler.Handle();
        }
    }
}