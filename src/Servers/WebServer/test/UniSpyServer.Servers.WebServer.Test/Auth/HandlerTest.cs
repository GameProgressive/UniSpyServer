using System.Collections.Generic;
using System.Net;
using Moq;
using UniSpyServer.Servers.WebServer.Entity.Structure;
using UniSpyServer.Servers.WebServer.Handler;
using UniSpyServer.Servers.WebServer.Test.Auth;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using Xunit;

namespace UniSpyServer.Servers.WebServer.Test
{
    public class HandlerTest
    {
        private IClient _client;
        public HandlerTest()
        {
            var serverMock = new Mock<IServer>();
            serverMock.Setup(s => s.ServerID).Returns(new System.Guid());
            serverMock.Setup(s => s.ServerName).Returns("WebServer");
            serverMock.Setup(s => s.Endpoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var sessionMock = new Mock<ITcpSession>();
            sessionMock.Setup(s => s.RemoteIPEndPoint).Returns(serverMock.Object.Endpoint);
            sessionMock.Setup(s => s.Server).Returns(serverMock.Object);
            sessionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Test);
            _client = new Client(sessionMock.Object);
        }
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