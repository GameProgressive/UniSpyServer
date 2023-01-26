using System.Net;
using Moq;
using UniSpyServer.Servers.Chat.Application;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Test
{
    public class TestClasses
    {
        public static IClient Client1;
        public static IClient Client2;

        static TestClasses()
        {
            Client1 = CreateClient("79.209.235.252", 50558);
            Client2 = CreateClient("211.83.127.54", 39503);
        }

        public static IClient CreateClient(string ipAddress = "79.209.235.252", int port = 50558)
        {
            var serverMock = new Mock<IServer>();
            serverMock.Setup(s => s.ServerID).Returns(new System.Guid());
            serverMock.Setup(s => s.ServerName).Returns("Chat");
            serverMock.Setup(s => s.ListeningEndPoint).Returns(new IPEndPoint(IPAddress.Any, 6666));
            var connectionMock = new Mock<ITcpConnection>();
            connectionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            connectionMock.Setup(s => s.Server).Returns(serverMock.Object);
            return new Client(connectionMock.Object);
        }
    }
}