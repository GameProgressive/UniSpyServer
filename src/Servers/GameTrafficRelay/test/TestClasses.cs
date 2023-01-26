// using System.Net;
// using Moq;
// using UniSpyServer.Servers.GameTrafficRelay.Application;
// using UniSpyServer.UniSpyLib.Abstraction.Interface;

// namespace UniSpyServer.Servers.GameTrafficRelay.Test
// {
//     public static class TestClasses
//     {
//         public static IClient Client = CreateClient();

//         public static Client CreateClient(string ipAddress = "192.168.1.1", int port = 9999)
//         {
//             var serverMock = new Mock<IServer>();
//             serverMock.Setup(s => s.ServerID).Returns(new System.Guid());
//             serverMock.Setup(s => s.ServerName).Returns("GameTrafficRelay");
//             serverMock.Setup(s => s.ListeningEndPoint).Returns(new IPEndPoint(IPAddress.Any, 99));
//             var connectionMock = new Mock<IUdpConnection>();
//             connectionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse(ipAddress), port));
//             connectionMock.Setup(s => s.Server).Returns(serverMock.Object);
//             connectionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Udp);

//             return new Client(connectionMock.Object);
//         }
//     }
// }