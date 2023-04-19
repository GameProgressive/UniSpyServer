using System.Net;
using Moq;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Test
{
    public class MockObject
    {
        public static IClient CreateClient(string ipAddress = "79.209.235.252", int port = 50558)
        {
            var managerMock = new Mock<IConnectionManager>();
            var connectionMock = new Mock<ITcpConnection>();
            connectionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            connectionMock.Setup(s => s.Manager).Returns(managerMock.Object);
            connectionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Tcp);
            var serverMock = new Chat.Application.Server(managerMock.Object);
            var client = new Client(connectionMock.Object, serverMock);
            client.Info.GameName = "gmtest";
            return client;
        }
    }
}