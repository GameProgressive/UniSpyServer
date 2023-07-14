using System.Threading;
using System.Net;
using Xunit;
using UniSpy.Server.GameTrafficRelay.Controller;
using System.Net.Sockets;
using UniSpy.Server.NatNegotiation.Aggregate.GameTrafficRelay;

namespace UniSpy.Server.GameTrafficRelay.Test
{
    public class GameTest
    {
        public GameTest()
        {
            MockObject.CreateServer();
        }

        [Fact]
        public void GetNagNegotiationInfo()
        {
            // var mockServer = new Mock<Core.Abstraction.Interface.IServer>();
            // mockServer.Setup(s => s.PublicIPEndPoint).Returns(IPEndPoint.Parse("202.91.0.1:123"));
            // ServerLauncher.ServerInstances.Add(mockServer.Object);

            var httpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();

            var controllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = httpContext
            };
            controllerContext.HttpContext.Connection.LocalIpAddress = IPAddress.Parse("127.0.0.1");
            var controller = new NatNegotiationController(null)
            {
                ControllerContext = controllerContext
            };

            var request = new NatNegotiationRequest()
            {
                Cookie = 123456,
                ServerId = System.Guid.NewGuid(),
                GameClientIPs = new System.Collections.Generic.List<string>() { "127.0.0.1:1234" },
                GameServerIPs = new System.Collections.Generic.List<string>() { "127.0.0.1:1235" }
            };

            var resp = controller.GetNatNegotiationInfo(request).Result;
            var relayEnd = new IPEndPoint(IPAddress.Loopback, resp.Port);

            var sockClient = new Socket(AddressFamily.InterNetwork,
                                  SocketType.Dgram,
                                  ProtocolType.Udp);
            sockClient.Bind(IPEndPoint.Parse("127.0.0.1:1234"));
            // we only use one client to send message to check if the listener shutdown
            var req = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
            sockClient.SendTo(req, relayEnd);
            var sockServer = new Socket(AddressFamily.InterNetwork,
                                  SocketType.Dgram,
                                  ProtocolType.Udp);
            sockServer.Bind(IPEndPoint.Parse("127.0.0.1:1235"));
            sockServer.SendTo(req, relayEnd);
            Thread.Sleep(2000);

            sockClient.SendTo(req, relayEnd);
            Thread.Sleep(2000);
            sockServer.SendTo(req, relayEnd);
            // // Thread.Sleep(10000);
            // // we check if listener is stoped
            // Assert.False(IsPortUsing(resp.IPEndPoint1.Port));
            // Assert.False(IsPortUsing(resp.IPEndPoint2.Port));
            // // because we are not runing server, so the server object is null
            // Assert.Null(resp.IPEndPoint1);
            // Assert.Null(resp.IPEndPoint2);
        }
    }
}