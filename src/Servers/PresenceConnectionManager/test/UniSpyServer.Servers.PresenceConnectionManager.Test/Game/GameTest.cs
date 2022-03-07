using System.Collections.Generic;
using System.Net;
using Moq;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure;
using UniSpyServer.Servers.PresenceConnectionManager.Handler;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;
using Xunit;

namespace UniSpyServer.Servers.PresenceConnectionManager.Test
{
    public class GameTest
    {
        private Client _client;
        public GameTest()
        {
            var serverMock = new Mock<IServer>();
            serverMock.Setup(s => s.ServerID).Returns(new System.Guid());
            serverMock.Setup(s => s.ServerName).Returns("PresenceConnectionManager");
            serverMock.Setup(s => s.Endpoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var sessionMock = new Mock<ITcpSession>();
            sessionMock.Setup(s => s.RemoteIPEndPoint).Returns(serverMock.Object.Endpoint);
            sessionMock.Setup(s => s.Server).Returns(serverMock.Object);
            sessionMock.Setup(s=>s.ConnectionType).Returns(NetworkConnectionType.Tcp);

            _client = new Client(sessionMock.Object);
        }

        [Fact]
        public void Civilization4()
        {
            var rawRequests = new List<string>()
            {
                @"\newuser\\email\civ4@unispy.org\nick\civ4-tk\passwordenc\JMHGwQ__\productid\10435\gamename\civ4\namespaceid\17\uniquenick\civ4-tk\id\1\final\",
                @"\login\\challenge\xMsHUXuWNXL3KMwmhoQZJrP0RVsArCYT\uniquenick\civ4-tk\userid\25\profileid\26\response\7f2c9c6685570ea18b7207d2cbd72452\firewall\1\port\0\productid\10435\gamename\civ4\namespaceid\17\sdkrevision\1\id\1\final\"
            };
            foreach (var raw in rawRequests)
            {
                new CmdSwitcher(_client, UniSpyEncoding.GetBytes(raw)).Switch();
            }
        }
    }
}