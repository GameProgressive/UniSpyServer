using UniSpy.Server.Core.Abstraction.Interface;
using System.Net;
using Moq;
using UniSpy.Server.PresenceConnectionManager.Application;

namespace UniSpy.Server.PresenceConnectionManager.Test
{
    public class TestClasses
    {
        /// <summary>
        /// PCM test client, create one here, use anywhere
        /// </summary>
        public static IClient client;
        static TestClasses()
        {
            client = CreateClient();
        }

        public static IClient CreateClient(string ipAddress = "192.168.1.1", int port = 9990)
        {
            var serverMock = new Mock<IServer>();
            serverMock.Setup(s => s.ServerID).Returns(new System.Guid());
            serverMock.Setup(s => s.ServerName).Returns("PresenceConnectionManager");
            serverMock.Setup(s => s.ListeningIPEndPoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var connectionMock = new Mock<ITcpConnection>();
            connectionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            connectionMock.Setup(s => s.Server).Returns(serverMock.Object);
            connectionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Tcp);
            client = new Client(connectionMock.Object);
            return client;
        }
    }
}