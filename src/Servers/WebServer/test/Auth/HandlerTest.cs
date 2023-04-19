using System.Collections.Generic;
using Moq;
using UniSpy.Server.WebServer.Handler;
using UniSpy.Server.WebServer.Test.Auth;
using UniSpy.Server.Core.Abstraction.Interface;
using Xunit;
using UniSpy.Server.WebServer.Application;

namespace UniSpy.Server.WebServer.Test
{
    public class HandlerTest
    {
        private Client _client = MokeObject.CreateClient();

        [Fact]
        public void SwitcherTest()
        {
            // Given
            var requests = new List<string>(){
                RawRequests.LoginProfile,
                RawRequests.LoginPs3Cert,
                RawRequests.LoginRemoteAuth,
                RawRequests.LoginUniqueNick
            };
            foreach (var req in requests)
            {
                var requestMock = new Mock<IHttpRequest>();
                requestMock.Setup(r => r.Body).Returns(req);
                var sw = new CmdSwitcher(_client, requestMock.Object);
                sw.Handle();
            }
        }
    }
}