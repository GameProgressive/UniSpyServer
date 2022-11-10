using System.Net;
using Moq;
using UniSpyServer.Servers.QueryReport.Entity.Structure;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.QueryReport.Test
{
    public class TestClasses
    {
        public static IClient Client = CreateClient();

        public static Client CreateClient(string ipAddress = "192.168.1.1", int port = 9999)
        {
            var serverMock = new Mock<IServer>();
            serverMock.Setup(s => s.ServerID).Returns(new System.Guid());
            serverMock.Setup(s => s.ServerName).Returns("QueryReport");
            serverMock.Setup(s => s.Endpoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var connectionMock = new Mock<IUdpConnection>();
            connectionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            connectionMock.Setup(s => s.Server).Returns(serverMock.Object);
            connectionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Udp);

            return new Client(connectionMock.Object);
        }
    }
}