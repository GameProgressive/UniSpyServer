using System.Net;
using Moq;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.GameTrafficRelay.Test
{
    public static class TestClasses
    {
        public static IServer Server = CreateServer();

        public static IServer CreateServer(string ipAddress = "192.168.1.1", int port = 9999)
        {
            var serverMock = new Mock<IServer>();
            serverMock.Setup(s => s.ServerID).Returns(new System.Guid());
            serverMock.Setup(s => s.ServerName).Returns("GameTrafficRelay");
            serverMock.Setup(s => s.ListeningIPEndPoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            serverMock.Setup(s => s.PublicIPEndPoint).Returns(IPEndPoint.Parse("101.34.77.11"));


            return (IServer)serverMock;
        }
    }
}