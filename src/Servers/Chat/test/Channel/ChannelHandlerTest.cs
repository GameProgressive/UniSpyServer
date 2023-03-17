using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Handler.CmdHandler.Channel;
using UniSpy.Server.Chat.Handler.CmdHandler.General;
using UniSpy.Server.Chat.Handler.CmdHandler.Message;
using Xunit;

namespace UniSpy.Server.Chat.Test.Channel
{
    public class ChannelHandlerTest
    {
        [Fact]
        public void JoinHandleTest()
        {
            var client1 = (Client)MockObject.CreateClient(port: 1234);
            var client2 = (Client)MockObject.CreateClient(port: 1235);
            SingleJoinTest(client1, "unispy1", "unispy1", "#GSP!room!test1");
            SingleJoinTest(client2, "unispy2", "unispy2", "#GSP!room!test1");
        }
        [Fact]
        public void ChannelMsgTest()
        {
            var client1 = (Client)MockObject.CreateClient(port: 1236);
            var client2 = (Client)MockObject.CreateClient(port: 1237);
            SingleJoinTest(client1, "unispy3", "unispy3", "#GSP!room!test2");
            SingleJoinTest(client2, "unispy4", "unispy4", "#GSP!room!test2");
            var privMsgReq = new PrivateRequest("PRIVMSG #GSP!room!test2 :hello this is a test.");
            var privMsgHandler = new PrivateHandler(client1, privMsgReq);
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
            var client = (Client)MockObject.CreateClient(port: 1238);

            SingleJoinTest(client, "spyguy6", "spyguy6","#GSP!room!test14");
            var request = new SetCKeyRequest(ChannelRequests.SetCKey);
            var handler = new SetCKeyHandler(client, request);
            handler.Handle();
        }
        [Fact]
        public void ModeTest()
        {
            var client = (Client)MockObject.CreateClient(port: 1239);

            SingleJoinTest(client, "spyguy7", "spyguy7", "#GSP!gmtest!MlNK4q4l1M");
            var request = new ModeRequest("MODE #GSP!gmtest!MlNK4q4l1M -i-p-s+m-n+t+l+e 2");
            var handler = new ModeHandler(client, request);
            handler.Handle();
        }
    }
}