using System.Net;
using Moq;
using UniSpyServer.Servers.Chat.Entity.Structure;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Test
{
    public class TestClasses
    {
        public static IClient Client1;
        public static IClient Client2;

        static TestClasses()
        {
            var serverMock = new Mock<IServer>();
            serverMock.Setup(s => s.ServerID).Returns(new System.Guid());
            serverMock.Setup(s => s.ServerName).Returns("Chat");
            serverMock.Setup(s => s.Endpoint).Returns(new IPEndPoint(IPAddress.Any, 6666));
            var sessionMock = new Mock<ITcpSession>();
            sessionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse("79.209.235.252"), 50558));
            sessionMock.Setup(s => s.Server).Returns(serverMock.Object);
            Client1 = new Client(sessionMock.Object);
            sessionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse("211.83.127.54"), 39503));
            Client2 = new Client(sessionMock.Object);
        }
    }
}