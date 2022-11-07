using System.Net;
using Moq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.GameTrafficRelay.Test
{
    public static class TestClasses
    {
        public static IClient Client = CreateQRClient();

        public static GameTrafficRelay.Entity.Structure.Client CreateQRClient(string ipAddress = "192.168.1.1", int port = 9999)
        {
            var qrServerMock = new Mock<IServer>();
            qrServerMock.Setup(s => s.ServerID).Returns(new System.Guid());
            qrServerMock.Setup(s => s.ServerName).Returns("GameTrafficRelay");
            qrServerMock.Setup(s => s.Endpoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var qrSessionMock = new Mock<IUdpSession>();
            qrSessionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            qrSessionMock.Setup(s => s.Server).Returns(qrServerMock.Object);
            qrSessionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Test);

            return new GameTrafficRelay.Entity.Structure.Client(qrSessionMock.Object);
        }
    }
}