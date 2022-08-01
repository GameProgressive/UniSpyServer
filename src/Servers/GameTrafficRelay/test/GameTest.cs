using System.Net;
using Moq;
using UniSpyServer.Servers.GameTrafficRelay.Entity.Structure;
using UniSpyServer.Servers.GameTrafficRelay.Handler;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using Xunit;

namespace UniSpyServer.Servers.GameTrafficRelay.Test
{
    public class GameTest
    {
        [Fact]
        public void PingNormalTest()
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
            var client = new TestClient(clientSessionMock.Object);

            var serverSessionMock = new Mock<IUdpSession>();
            serverSessionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse("192.168.1.3"), 9998));
            serverSessionMock.Setup(s => s.Server).Returns(serverMock.Object);
            serverSessionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Udp);
            serverSessionMock.Setup(s => s.SessionExistedTime).Returns(new System.TimeSpan(0, 0, 0, 0, 0));
            var server = new TestClient(serverSessionMock.Object);

            var clientRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
            var serverRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x00, 0x67, 0x6C, 0xFD, 0x00, 0x00 };

            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);

            client.OnReceived(clientRequest);
            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);
            server.OnReceived(serverRequest);
            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);
            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);
            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);

            Assert.True(client.Info.TrafficRelayTarget.Session.RemoteIPEndPoint.Equals(server.Session.RemoteIPEndPoint));
            Assert.True(server.Info.TrafficRelayTarget.Session.RemoteIPEndPoint.Equals(client.Session.RemoteIPEndPoint));
        }

        [Fact]
        /// <summary>
        /// A new ping packet with same ip and port incoming
        /// </summary>
        public void UpdatedPacketPingTest()
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
            var client = new TestClient(clientSessionMock.Object);

            var serverSessionMock = new Mock<IUdpSession>();
            serverSessionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse("192.168.1.3"), 9998));
            serverSessionMock.Setup(s => s.Server).Returns(serverMock.Object);
            serverSessionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Udp);
            serverSessionMock.Setup(s => s.SessionExistedTime).Returns(new System.TimeSpan(0, 0, 0, 0, 0));
            var server = new TestClient(serverSessionMock.Object);

            var clientRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
            var serverRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x00, 0x67, 0x6C, 0xFD, 0x00, 0x00 };

            TestClient.ClientPool.Add(client.Session.RemoteIPEndPoint, client);
            client.OnReceived(clientRequest);
            TestClient.ClientPool.Add(server.Session.RemoteIPEndPoint, server);
            server.OnReceived(serverRequest);

            client.OnReceived(clientRequest);
            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);
            server.OnReceived(serverRequest);
            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);
            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);
            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);

            // new ping packet with same IP and port
            clientRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9B, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
            serverRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9B, 0xC0, 0xA8, 0x00, 0x67, 0x6C, 0xFD, 0x00, 0x00 };

            client.OnReceived(clientRequest);
            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);
            server.OnReceived(serverRequest);
            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);
            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);
            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);


            Assert.True(client.Info.TrafficRelayTarget.Session.RemoteIPEndPoint == server.Session.RemoteIPEndPoint);
            Assert.True(server.Info.TrafficRelayTarget.Session.RemoteIPEndPoint == client.Session.RemoteIPEndPoint);
        }

        [Fact]
        /// <summary>
        /// New ping packet with changed server ip and port
        /// </summary>
        public void JuliusRealTest()
        {
            var serverMock = new Mock<IServer>();
            serverMock.Setup(s => s.ServerID).Returns(new System.Guid());
            serverMock.Setup(s => s.ServerName).Returns("GameTrafficRelay");
            serverMock.Setup(s => s.Endpoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var clientSessionMock = new Mock<IUdpSession>();
            clientSessionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse("79.209.235.252"), 52577));
            clientSessionMock.Setup(s => s.Server).Returns(serverMock.Object);
            clientSessionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Udp);
            clientSessionMock.Setup(s => s.SessionExistedTime).Returns(new System.TimeSpan(0, 0, 0, 0, 0));
            var client = new TestClient(clientSessionMock.Object);

            var serverSessionMock = new Mock<IUdpSession>();
            serverSessionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse("211.83.127.54"), 50816));
            serverSessionMock.Setup(s => s.Server).Returns(serverMock.Object);
            serverSessionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Udp);
            serverSessionMock.Setup(s => s.SessionExistedTime).Returns(new System.TimeSpan(0, 0, 0, 0, 0));
            var server = new TestClient(serverSessionMock.Object);

            var clientRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xA1, 0x61, 0x8D, 0x59, 0x27, 0x66, 0x00, 0x00 };
            var serverRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xA1, 0x61, 0x8D, 0x59, 0x27, 0x66, 0x00, 0x00 };

            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);

            client.OnReceived(clientRequest);
            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);
            server.OnReceived(serverRequest);
            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);
            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);
            client.OnReceived(clientRequest);
            server.OnReceived(serverRequest);

            Assert.True(client.Info.TrafficRelayTarget.Session.RemoteIPEndPoint.Equals(server.Session.RemoteIPEndPoint));
            Assert.True(server.Info.TrafficRelayTarget.Session.RemoteIPEndPoint.Equals(client.Session.RemoteIPEndPoint));
        }
    }
}