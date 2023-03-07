using Xunit;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Test.Message
{
    public class MessageRequestTest
    {

        [Fact]
        public void AboveTheTableMsg()
        {
            var request = new AtmRequest(MessageRequests.AboveTheTableMsg);
            request.Parse();
            Assert.Equal(MessageType.ChannelMessage, request.Type);
            Assert.Null(request.NickName);
            Assert.Equal("#GSP!room!test", request.ChannelName);
        }

        [Fact]
        public void Notice()
        {
            var request = new NoticeRequest(MessageRequests.Notice);
            request.Parse();
            Assert.Equal(MessageType.ChannelMessage, request.Type);
            Assert.Null(request.NickName);
            Assert.Equal("#GSP!room!test", request.ChannelName);
        }

        [Fact]
        public void PrivateMsg()
        {
            var request = new PrivateRequest(MessageRequests.PrivateMsg);
            request.Parse();
            Assert.Equal(MessageType.ChannelMessage, request.Type);
            Assert.Null(request.NickName);
            Assert.Equal("#GSP!room!test", request.ChannelName);
        }

        [Fact]
        public void UnderTheTableMsg()
        {
            var request = new UtmRequest(MessageRequests.UnderTheTableMsg);
            request.Parse();
            Assert.Equal(MessageType.ChannelMessage, request.Type);
            Assert.Null(request.NickName);
            Assert.Equal("#GSP!room!test", request.ChannelName);
        }
    }
}
