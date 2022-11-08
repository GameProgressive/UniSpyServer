using System.Net;
using Moq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.GameStatus.Test
{
    public static class TestClasses
    {
        public static IClient Client = CreateGSClient();

        public static GameStatus.Entity.Structure.Client CreateGSClient(string ipAddress = "192.168.1.1", int port = 9999)
        {
            var qrServerMock = new Mock<IServer>();
            qrServerMock.Setup(s => s.ServerID).Returns(new System.Guid());
            qrServerMock.Setup(s => s.ServerName).Returns("GameStatus");
            qrServerMock.Setup(s => s.Endpoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var qrSessionMock = new Mock<ITcpSession>();
            qrSessionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            qrSessionMock.Setup(s => s.Server).Returns(qrServerMock.Object);
            qrSessionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Test);

            return new GameStatus.Entity.Structure.Client(qrSessionMock.Object);
        }
    }
}