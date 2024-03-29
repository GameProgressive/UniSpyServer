using System.Net;
using Moq;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.WebServer.Application;

namespace UniSpy.Server.WebServer.Test
{
    public static class MokeObject
    {
        public static IClient Client = CreateClient();

        public static Client CreateClient(string ipAddress = "192.168.1.2", int port = 9999)
        {
            var managerMock = new Mock<IConnectionManager>();
            var connectionMock = new Mock<IHttpConnection>();
            connectionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            connectionMock.Setup(s => s.Manager).Returns(managerMock.Object);
            connectionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Http);
            var serverMock = new WebServer.Application.Server(managerMock.Object);
            return new Client(connectionMock.Object, serverMock);
        }
    }
}