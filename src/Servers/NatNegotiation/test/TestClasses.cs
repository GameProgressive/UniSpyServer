using System.Net;
using Moq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.NatNegotiation.Test
{
    public static class TestClasses
    {
        public static IClient Client = CreateNatNegClient();

        public static NatNegotiation.Entity.Structure.Client CreateNatNegClient(string ipAddress = "192.168.1.1", int port = 9999)
        {
            var qrServerMock = new Mock<IServer>();
            qrServerMock.Setup(s => s.ServerID).Returns(new System.Guid());
            qrServerMock.Setup(s => s.ServerName).Returns("NatNegotiation");
            qrServerMock.Setup(s => s.Endpoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var qrSessionMock = new Mock<IUdpSession>();
            qrSessionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            qrSessionMock.Setup(s => s.Server).Returns(qrServerMock.Object);
            qrSessionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Test);

            return new NatNegotiation.Entity.Structure.Client(qrSessionMock.Object);
        }
    }
}