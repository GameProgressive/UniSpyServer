using System.Net;
using Moq;
using UniSpy.Server.ServerBrowser.Application;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.ServerBrowser.Test
{
    public static class TestClasses
    {
        public static IClient QRClient = QueryReport.Test.TestClasses.CreateClient();
        public static IClient SBClient = CreateClient();

        public static Client CreateClient(string ipAddress = "192.168.1.2", int port = 9999)
        {
            var serverMock = new Mock<IServer>();
            serverMock.Setup(s => s.ServerID).Returns(new System.Guid());
            serverMock.Setup(s => s.ServerName).Returns("ServerBrowser");
            serverMock.Setup(s => s.ListeningIPEndPoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var connectionMock = new Mock<ITcpConnection>();
            connectionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            connectionMock.Setup(s => s.Server).Returns(serverMock.Object);
            connectionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Tcp);
            return new Client(connectionMock.Object);
        }
    }
}