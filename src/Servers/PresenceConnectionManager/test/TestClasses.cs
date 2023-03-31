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
            var managerMock = new Mock<IConnectionManager>();
            var connectionMock = new Mock<ITcpConnection>();
            connectionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            connectionMock.Setup(s => s.Manager).Returns(managerMock.Object);
            connectionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Tcp);
            var serverMock = new PresenceConnectionManager.Application.Server(managerMock.Object);

            return new Client(connectionMock.Object, serverMock);
        }
    }
}