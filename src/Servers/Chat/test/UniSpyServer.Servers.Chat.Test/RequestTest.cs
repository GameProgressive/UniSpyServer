using System;
using System.Linq;
using Chat.Entity.Contract;
using Chat.Entity.Structure.Request.General;
using Xunit;

namespace UniSpyServer.Servers.Chat.Test
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
