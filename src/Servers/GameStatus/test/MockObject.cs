using System.Net;
using Moq;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.GameStatus.Application;

namespace UniSpy.Server.GameStatus.Test
{
    public static class MockObject
    {
        public static IClient Client = CreateClient();
        public static Client CreateClient(string ipAddress = "192.168.1.1", int port = 9999)
        {

            var managerMock = new Mock<IConnectionManager>();
            var connectionMock = new Mock<ITcpConnection>();
            connectionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            connectionMock.Setup(s => s.Manager).Returns(managerMock.Object);
            connectionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Tcp);
            var serverMock = new GameStatus.Application.Server(managerMock.Object);

            return new Client(connectionMock.Object, serverMock);
        }
    }
}