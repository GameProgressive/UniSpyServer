using System.Collections.Generic;
using System.Net;
using Moq;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;
using UniSpyServer.Servers.NatNegotiation.Handler;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using Xunit;

namespace UniSpyServer.Servers.GameTrafficRelay.Test
{
    public class GameTest
    {
        [Fact]
        public void PingTest()
        {
            var serverMock = new Mock<IServer>();
            serverMock.Setup(s => s.ServerID).Returns(new System.Guid());
            serverMock.Setup(s => s.ServerName).Returns("GameTrafficRelay");
            serverMock.Setup(s => s.Endpoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var clientSessionMock = new Mock<IUdpSession>();
            clientSessionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse("192.168.1.2"), 9999));
            clientSessionMock.Setup(s => s.Server).Returns(serverMock.Object);
            clientSessionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Udp);
            clientSessionMock.Setup(s => s.SessionExistedTime).Returns(new System.TimeSpan(0, 0, 0, 0, 0));
            var client = new Client(clientSessionMock.Object);

            var serverSessionMock = new Mock<IUdpSession>();
            serverSessionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse("192.168.1.3"), 9999));
            serverSessionMock.Setup(s => s.Server).Returns(serverMock.Object);
            serverSessionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Udp);
            serverSessionMock.Setup(s => s.SessionExistedTime).Returns(new System.TimeSpan(0, 0, 0, 0, 0));
            var server = new Client(serverSessionMock.Object);

            var clientRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
            var serverRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x00, 0x67, 0x6C, 0xFD, 0x00, 0x00 };


            var clientRequests = new List<byte[]>() { clientRequest, clientRequest, clientRequest };
            var serverRequests = new List<byte[]>() { serverRequest, serverRequest, serverRequest };

            foreach (var request in clientRequests)
            {
                var switcher = new CmdSwitcher(client, request);
                switcher.Switch();
            }

            foreach (var request in serverRequests)
            {
                var switcher = new CmdSwitcher(server, request);
                switcher.Switch();
            }
        }
    }
}