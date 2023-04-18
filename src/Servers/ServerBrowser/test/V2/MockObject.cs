using System.Net;
using Moq;
using UniSpy.Server.ServerBrowser.V2.Application;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.ServerBrowser.V2.Test
{
    public static class MockObject
    {
        public static IClient QRClient = QueryReport.V2.Test.MockObject.CreateClient();
        public static IClient SBClient = CreateClient();

        public static Client CreateClient(string ipAddress = "192.168.1.2", int port = 9999)
        {
            var managerMock = new Mock<IConnectionManager>();
            var connectionMock = new Mock<ITcpConnection>();
            connectionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            connectionMock.Setup(s => s.Manager).Returns(managerMock.Object);
            connectionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Tcp);
            var serverMock = new ServerBrowser.V2.Application.Server(managerMock.Object);

            return new Client(connectionMock.Object, serverMock);
        }
    }
}