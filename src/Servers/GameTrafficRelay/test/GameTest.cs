using System.Net;
using Moq;
using UniSpyServer.Servers.GameTrafficRelay.Entity.Structure;
using UniSpyServer.Servers.GameTrafficRelay.Handler;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using Xunit;

namespace UniSpyServer.Servers.GameTrafficRelay.Test
{
    public class GameTest
    {
        [Fact]
        public void PingNormalTest()
        {

            var client = TestClasses.CreateQRClient("192.168.1.2", 9999);
            var server = TestClasses.CreateQRClient("192.168.1.3", 9999);

            var clientRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
            var serverRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x00, 0x67, 0x6C, 0xFD, 0x00, 0x00 };

            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);

            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            
            Assert.True(client.Info.TrafficRelayTarget.Session.RemoteIPEndPoint.Equals(server.Session.RemoteIPEndPoint));
            Assert.True(server.Info.TrafficRelayTarget.Session.RemoteIPEndPoint.Equals(client.Session.RemoteIPEndPoint));
        }

        [Fact]
        /// <summary>
        /// A new ping packet with same ip and port incoming
        /// </summary>
        public void UpdatedPacketPingTest()
        {
            var client = TestClasses.CreateQRClient("192.168.1.2", 9999);
            var server = TestClasses.CreateQRClient("192.168.1.3", 9999);

            var clientRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
            var serverRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x00, 0x67, 0x6C, 0xFD, 0x00, 0x00 };

            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);

            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);

            // new ping packet with same IP and port
            clientRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9B, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
            serverRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9B, 0xC0, 0xA8, 0x00, 0x67, 0x6C, 0xFD, 0x00, 0x00 };

            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);


            Assert.True(client.Info.TrafficRelayTarget.Session.RemoteIPEndPoint == server.Session.RemoteIPEndPoint);
            Assert.True(server.Info.TrafficRelayTarget.Session.RemoteIPEndPoint == client.Session.RemoteIPEndPoint);
        }

        [Fact]
        /// <summary>
        /// New ping packet with changed server ip and port
        /// </summary>
        public void JuliusRealTest()
        {
            var client = TestClasses.CreateQRClient("79.209.235.252", 52577);
            var server = TestClasses.CreateQRClient("211.83.127.54", 50816);
            var clientRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xA1, 0x61, 0x8D, 0x59, 0x27, 0x66, 0x00, 0x00 };
            var serverRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xA1, 0x61, 0x8D, 0x59, 0x27, 0x66, 0x00, 0x00 };

            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);

            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);
            ((ITestClient)client).TestReceived(clientRequest);
            ((ITestClient)server).TestReceived(serverRequest);

            Assert.True(client.Info.TrafficRelayTarget.Session.RemoteIPEndPoint.Equals(server.Session.RemoteIPEndPoint));
            Assert.True(server.Info.TrafficRelayTarget.Session.RemoteIPEndPoint.Equals(client.Session.RemoteIPEndPoint));
        }
    }
}