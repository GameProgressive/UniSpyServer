using System.Net;
using Moq;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.WebServer.Application;

namespace UniSpy.Server.WebServer.Test
{
    public static class TestClasses
    {
        public static IClient Client = CreateClient();

        public static Client CreateClient(string ipAddress = "192.168.1.2", int port = 9999)
        {
            var serverMock = new Mock<IServer>();
            serverMock.Setup(s => s.ServerID).Returns(new System.Guid());
            serverMock.Setup(s => s.ServerName).Returns("WebServer");
            serverMock.Setup(s => s.ListeningIPEndPoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var connectionMock = new Mock<IHttpConnection>();
            connectionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            connectionMock.Setup(s => s.Server).Returns(serverMock.Object);
            connectionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Http);

            return new Client(connectionMock.Object);
        }
    }
}