using Xunit;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Test.Message
{
    public class MessageRequestTest
    {

        [Fact(Skip = "TODO: add tests")]
        public void AboveTheTableMsg()
        {
            var request = new AboveTheTableMsgRequest(MessageRequests.AboveTheTableMsg);
            request.Parse();
        }

        [Fact(Skip = "TODO: add tests")]
        public void Notice()
        {
            var request = new NoticeRequest(MessageRequests.Notice);
            request.Parse();
        }

        [Fact]
        public void PrivateMsg()
        {
            var request = new PrivateMsgRequest(MessageRequests.PrivateMsg);
            request.Parse();
            Assert.Equal(MessageType.ChannelMessage, request.MessageType);
            Assert.Null(request.NickName);
            Assert.Equal("#GSP!room!test", request.ChannelName);
        }

        [Fact(Skip = "TODO: add tests")]
        public void UnderTheTableMsg()
        {
            var request = new UnderTheTableMsgRequest(MessageRequests.UnderTheTableMsg);
            request.Parse();
        }
    }
}
