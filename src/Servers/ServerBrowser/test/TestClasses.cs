using System.Net;
using Moq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.ServerBrowser.Test
{
    public static class TestClasses
    {
        public static IClient QRClient = QueryReport.Test.TestClasses.CreateQRClient();
        public static IClient SBClient = CreateSBClient();

        public static ServerBrowser.Entity.Structure.Client CreateSBClient(string ipAddress = "192.168.1.2", int port = 9999)
        {
            var sbServerMock = new Mock<IServer>();
            sbServerMock.Setup(s => s.ServerID).Returns(new System.Guid());
            sbServerMock.Setup(s => s.ServerName).Returns("ServerBrowser");
            sbServerMock.Setup(s => s.Endpoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var sbSessionMock = new Mock<IUdpSession>();
            sbSessionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse("192.168.1.2"), 9999));
            sbSessionMock.Setup(s => s.Server).Returns(sbServerMock.Object);
            sbSessionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Test);
            return new ServerBrowser.Entity.Structure.Client(sbSessionMock.Object);
        }
    }
}