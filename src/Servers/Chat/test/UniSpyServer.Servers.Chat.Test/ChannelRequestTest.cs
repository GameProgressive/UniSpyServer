using Xunit;
using UniSpyServer.Chat.Entity.Structure.Request.Channel;

namespace UniSpyServer.Servers.UniSpyServer.Chat.RequestTest
{
    public class ChannelRequestTest
    {
        //FIXME:
        /*[Fact]
        public void GetChannelKeyRequestTest()
        {
            var rawRequest = "GETCHANKEY XXXX";
            var request = new GetChannelKeyRequest(rawRequest);
            request.Parse();
            Assert.Equal("XXXX", request.Cookie);
            //TODO: add _longParam to Assert.Equal("XXXX", request.Keys);
        }*/

        // TODO: add GetCKeyRequest

        //FIXME:
        /*[Fact]
        public void JoinRequestTest()
        {
            var rawRequest = "JOIN XXXXX";
            var request = new JoinRequest(rawRequest);
            request.Parse();
            Assert.Equal("XXXXX", request.Password);
        }*/

        // TODO: add _longParam
        /*[Fact]
        public void KickRequestTest()
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
