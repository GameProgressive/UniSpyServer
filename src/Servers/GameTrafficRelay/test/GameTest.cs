using System.Collections.Generic;
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

            var client1 = TestClasses.CreateQRClient("192.168.1.2", 9999);
            var client2 = TestClasses.CreateQRClient("192.168.1.3", 9999);

            var client1Request = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
            var client2Request = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x00, 0x67, 0x6C, 0xFD, 0x00, 0x00 };

            var requests = new List<KeyValuePair<string, byte[]>>()
            {
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
            };
            var clients = new Dictionary<string, IClient>()
            {
                {"client1",client1},
                {"client2",client2}
            };

            foreach (var req in requests)
            {
                ((ITestClient)clients[req.Key]).TestReceived(req.Value);
            }

            Assert.True(client1.Info.TrafficRelayTarget.Session.RemoteIPEndPoint.Equals(client2.Session.RemoteIPEndPoint));
            Assert.True(client2.Info.TrafficRelayTarget.Session.RemoteIPEndPoint.Equals(client1.Session.RemoteIPEndPoint));
        }

        [Fact]
        /// <summary>
        /// A new ping packet with same ip and port incoming
        /// </summary>
        public void UpdatedPacketPingTest()
        {
            var client1 = TestClasses.CreateQRClient("192.168.1.2", 9999);
            var client2 = TestClasses.CreateQRClient("192.168.1.3", 9999);

            var client1Request = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
            var client2Request = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x00, 0x67, 0x6C, 0xFD, 0x00, 0x00 };

            var requests = new List<KeyValuePair<string, byte[]>>()
            {
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
            };
            var clients = new Dictionary<string, IClient>()
            {
                {"client1",client1},
                {"client2",client2}
            };

            foreach (var req in requests)
            {
                ((ITestClient)clients[req.Key]).TestReceived(req.Value);
            }

            // new ping packet with same IP and port
            client1Request = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9B, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
            client2Request = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9B, 0xC0, 0xA8, 0x00, 0x67, 0x6C, 0xFD, 0x00, 0x00 };

            requests = new List<KeyValuePair<string, byte[]>>()
            {
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
            };


            foreach (var req in requests)
            {
                ((ITestClient)clients[req.Key]).TestReceived(req.Value);
            }

            Assert.True(client1.Info.TrafficRelayTarget.Session.RemoteIPEndPoint == client2.Session.RemoteIPEndPoint);
            Assert.True(client2.Info.TrafficRelayTarget.Session.RemoteIPEndPoint == client1.Session.RemoteIPEndPoint);
        }

        [Fact]
        /// <summary>
        /// New ping packet with changed server ip and port
        /// </summary>
        public void JuliusRealTest()
        {
            var client1 = TestClasses.CreateQRClient("79.209.235.252", 52577);
            var client2 = TestClasses.CreateQRClient("211.83.127.54", 50816);
            var client1Request = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xA1, 0x61, 0x8D, 0x59, 0x27, 0x66, 0x00, 0x00 };
            var client2Request = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xA1, 0x61, 0x8D, 0x59, 0x27, 0x66, 0x00, 0x00 };

            var requests = new List<KeyValuePair<string, byte[]>>()
            {
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
                new KeyValuePair<string,byte[]>("client1",client1Request),
                new KeyValuePair<string,byte[]>("client2",client2Request),
            };

            var clients = new Dictionary<string, IClient>()
            {
                {"client1",client1},
                {"client2",client2}
            };

            foreach (var req in requests)
            {
                ((ITestClient)clients[req.Key]).TestReceived(req.Value);
            }
            Assert.True(client1.Info.TrafficRelayTarget.Session.RemoteIPEndPoint.Equals(client2.Session.RemoteIPEndPoint));
            Assert.True(client2.Info.TrafficRelayTarget.Session.RemoteIPEndPoint.Equals(client1.Session.RemoteIPEndPoint));
        }
    }
}