using System.Net;
using Moq;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.GameTrafficRelay.Test
{
    public static class TestClasses
    {
        public static IServer Server = CreateServer();

        public static IServer CreateServer(string ipAddress = "192.168.1.1", int port = 9999)
        {
            var managerMock = new Mock<IConnectionManager>();
            var connectionMock = new Mock<ITcpConnection>();
            connectionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            connectionMock.Setup(s => s.Manager).Returns(managerMock.Object);
            connectionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Tcp);
            var serverMock = new GameTrafficRelay.Application.Server();
            return serverMock;
        }
    }
}