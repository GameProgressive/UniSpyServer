using System.Collections.Generic;
using System.Net;
using Moq;
using UniSpy.Server.WebServer.Application;
using UniSpy.Server.WebServer.Handler;
using UniSpy.Server.WebServer.Test.Auth;
using UniSpy.Server.Core.Abstraction.Interface;
using Xunit;
using System.Linq;

namespace UniSpy.Server.WebServer.Test
{
    public class HandlerTest
    {
        private IClient _client = TestClasses.CreateClient();

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
                sw.Switch();
            }
        }
    }
}