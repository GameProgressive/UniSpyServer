using System.Net;
using Moq;
using UniSpy.Server.NatNegotiation.Application;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.NatNegotiation.Test
{
    public static class MockObject
    {
        public static IClient Client = CreateClient();

        public static Client CreateClient(string ipAddress = "192.168.1.1", int port = 9999)
        {
            var managerMock = new Mock<IConnectionManager>();
            var connectionMock = new Mock<IUdpConnection>();
            connectionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            connectionMock.Setup(s => s.Manager).Returns(managerMock.Object);
            connectionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Udp);
            var serverMock = new NatNegotiation.Application.Server(managerMock.Object);

            return new Client(connectionMock.Object, serverMock);
        }
    }
}