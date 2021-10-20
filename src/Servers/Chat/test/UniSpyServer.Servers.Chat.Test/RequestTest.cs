using System;
using UniSpyServer.Chat.Entity.Structure.Request.General;
using Xunit;

namespace UniSpyServer.Servers.UniSpyServer.Chat.Test
{
    public class RequestTest
    {
        [Fact]
        public void NickRequestTest()
        {
            var raw = "NICK Wiz";
            var request = new NickRequest(raw);
            request.Parse();
            Assert.Equal("Wiz", request.NickName);   
        }
    }
}
