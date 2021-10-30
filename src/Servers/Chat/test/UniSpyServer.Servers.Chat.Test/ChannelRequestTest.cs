using Xunit;
using UniSpyServer.Chat.Entity.Structure.Request.Channel;

namespace UniSpyServer.Servers.UniSpyServer.Chat.Test.RequestTest
{
    public class ChannelRequestTest
    {
        //FIXME:
        /*[Fact]
        public void GetChannelKey()
        {
            var rawRequest = "GETCHANKEY XXXX";
            var request = new GetChannelKeyRequest(rawRequest);
            request.Parse();
            Assert.Equal("XXXX", request.Cookie);
            //TODO: add _longParam to Assert.Equal("XXXX", request.Keys);
        }*/

        // TODO: add GetCKeyRequest

        //FIXME:
        [Fact]
        public void Join()
        {
            var rawRequest = "JOIN #istanbul";
            var request = new JoinRequest(rawRequest);
            request.Parse();
            Assert.Equal("#istanbul", request.ChannelName);

            rawRequest = "JOIN #istanbul password";
            request = new JoinRequest(rawRequest);
            request.Parse();
            Assert.Equal("#istanbul", request.ChannelName);
            Assert.Equal("password", request.Password);
        }

        // TODO: add _longParam
        /*[Fact]
        public void Kick()
        {
            var rawRequest = "KICK SpyGuy Spam";
            var request = new KickRequest(rawRequest);
            request.Parse();
            Assert.Equal("SpyGuy", request.KickeeNickName);
            Assert.Equal("Spam", request.Reason);
        }*/

        // TODO: add ModeRequest

        // TODO: add PartRequest

        // TODO: add SetChannelKeyRequest

        // TODO: add SetCKeyRequest

        // TODO: add TopicRequest
    }
}
