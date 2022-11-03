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
            var sessionMock = new Mock<ITcpSession>();
            sessionMock.Setup(s => s.RemoteIPEndPoint).Returns(serverMock.Object.Endpoint);
            sessionMock.Setup(s => s.Server).Returns(serverMock.Object);
            sessionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Tcp);
            Client = new Client(sessionMock.Object);
        }
    }
}