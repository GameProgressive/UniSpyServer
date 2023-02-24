using System.Net;
using Moq;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Handler.CmdHandler.Channel;
using UniSpy.Server.Chat.Handler.CmdHandler.General;
using UniSpy.Server.Chat.Handler.CmdHandler.Message;
using UniSpy.Server.Core.Abstraction.Interface;
using Xunit;

namespace UniSpy.Server.Chat.Test.Channel
{
    public class ChannelHandlerTest
    {
        private Client _client => (Client)TestClasses.CreateClient();
        public ChannelHandlerTest()
        {
            var serverMock = new Mock<IServer>();
            serverMock.Setup(s => s.ServerID).Returns(new System.Guid());
            serverMock.Setup(s => s.ServerName).Returns("Chat");
            serverMock.Setup(s => s.ListeningIPEndPoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var connectionMock = new Mock<ITcpConnection>();
            connectionMock.Setup(s => s.RemoteIPEndPoint).Returns(serverMock.Object.ListeningIPEndPoint);
            connectionMock.Setup(s => s.Server).Returns(serverMock.Object);
            connectionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Tcp);
        }
        [Fact]
        public void JoinHandleTest()
        {
            var client1 = (Client)TestClasses.CreateClient(port: 1234);
            var client2 = (Client)TestClasses.CreateClient(port: 1235);
            SingleJoinTest(client1, "unispy1", "unispy1", "#GSP!room!test");
            SingleJoinTest(client2, "unispy2", "unispy2", "#GSP!room!test");
        }
        [Fact]
        public void ChannelMsgTest()
        {
            var client1 = (Client)TestClasses.CreateClient(port: 1234);
            var client2 = (Client)TestClasses.CreateClient(port: 1235);
            SingleJoinTest(client1, "unispy1", "unispy1", "#GSP!room!test");
            SingleJoinTest(client2, "unispy2", "unispy2", "#GSP!room!test");
            var privMsgReq = new PrivateMsgRequest("PRIVMSG #GSP!room!test :hello this is a test.");
            var privMsgHandler = new PrivateMsgHandler(client1, privMsgReq);
            privMsgHandler.Handle();
        }
        private void SingleLoginTest(Client client, string userName = "unispy", string nickName = "unispy")
        {
            var userReq = new UserRequest($"USER {userName} 127.0.0.1 peerchat.unispy.org :{userName}");
            var nickReq = new NickRequest($"NICK {nickName}");
            var userHandler = new UserHandler(client, userReq);
            var nickHandler = new NickHandler(client, nickReq);
            userHandler.Handle();
            nickHandler.Handle();
        }
        private void SingleJoinTest(Client client, string userName = "unispy", string nickName = "unispy", string channelName = "#GSP!room!test")
        {
            SingleLoginTest(client, userName, nickName);
            var joinReq = new JoinRequest($"JOIN {channelName}");
            var joinHandler = new JoinHandler(client, joinReq);
            // we know the endpoint object is not set, so System.NullReferenceException will be thrown
            joinHandler.Handle();
            Assert.Single(client.Info.JoinedChannels);
            Assert.True(client.Info.JoinedChannels.ContainsKey(channelName));
            Assert.True(client.Info.IsJoinedChannel(channelName));
        }
        [Fact]
        public void SetCKeyTest()
        {
            SingleJoinTest(_client, "spyguy", "spyguy");
            var request = new SetCKeyRequest(ChannelRequests.SetCKey);
            var handler = new SetCKeyHandler(_client, request);
            handler.Handle();
        }
        [Fact]
        public void ModeTest()
        {
            SingleJoinTest(_client, "spyguy", "spyguy", "#GSP!gmtest!MlNK4q4l1M");
            var request = new ModeRequest("MODE #GSP!gmtest!MlNK4q4l1M -i-p-s+m-n+t+l+e 2");
            var handler = new ModeHandler(_client, request);
            handler.Handle();
        }
    }
}