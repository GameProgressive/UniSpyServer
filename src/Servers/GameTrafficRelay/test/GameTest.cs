using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;
using UniSpy.Server.GameTrafficRelay.Controller;
using System.Net.NetworkInformation;

namespace UniSpy.Server.GameTrafficRelay.Test
{
    public class GameTest
    {
        [Fact]
        public void GetNagNegotiationInfo()
        {
            // var connectionInfo = new DefaultConnectionInfo
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
                ServerId = System.Guid.NewGuid()
            };


            Assert.ThrowsAsync<System.NullReferenceException>(() => controller.GetNatNegotiationInfo(request));
            // var resp = controller.GetNatNegotiationInfo(request);

            // Assert.True(IsPortUsing(resp.IPEndPoint1.Port));
            // Assert.True(IsPortUsing(resp.IPEndPoint2.Port));
            // var sock = new Socket(AddressFamily.InterNetwork,
            //                       SocketType.Dgram,
            //                       ProtocolType.Udp);
            // // we only use one client to send message to check if the listener shutdown
            // var req = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
            // sock.SendTo(req, resp.IPEndPoint1);
            // Thread.Sleep(10000);
            // // we check if listener is stoped
            // Assert.False(IsPortUsing(resp.IPEndPoint1.Port));
            // Assert.False(IsPortUsing(resp.IPEndPoint2.Port));
            // // because we are not runing server, so the server object is null
            // Assert.Null(resp.IPEndPoint1);
            // Assert.Null(resp.IPEndPoint2);

        }
        internal static bool IsPortUsing(int port)
        {
            var tcpPorts = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Select(p => p.Port).ToArray();
            var udpPorts = IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners().Select(p => p.Port).ToArray();
            var usedPorts = new List<int>();
            usedPorts.AddRange(tcpPorts);
            usedPorts.AddRange(udpPorts);
            usedPorts.Sort();
            usedPorts.Distinct();
            if (usedPorts.Contains(port))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}