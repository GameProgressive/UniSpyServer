using UniSpyServer.UniSpyLib.Abstraction.Interface;
using System.Net;
using Moq;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure;

namespace UniSpyServer.Servers.PresenceConnectionManager.Test
{
    public class TestClasses
    {
        /// <summary>
        /// PCM test client, create one here, use anywhere
        /// </summary>
        public static IClient Client;
        static TestClasses()
        {
            var serverMock = new Mock<IServer>();
            serverMock.Setup(s => s.ServerID).Returns(new System.Guid());
            serverMock.Setup(s => s.ServerName).Returns("PresenceConnectionManager");
            serverMock.Setup(s => s.Endpoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var connectionMock = new Mock<ITcpConnection>();
            connectionMock.Setup(s => s.RemoteIPEndPoint).Returns(serverMock.Object.Endpoint);
            connectionMock.Setup(s => s.Server).Returns(serverMock.Object);
            connectionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Tcp);
            Client = new Client(connectionMock.Object);
        }
    }
}