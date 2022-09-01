using System.Collections.Generic;
using System.Net;
using Moq;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure;
using UniSpyServer.Servers.ServerBrowser.Handler;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using Xunit;

namespace UniSpyServer.Servers.UniSpyServer.Servers.ServerBrowser.Test
{
    public class GameTest
    {
        [Fact]
        public void GameSpyExampleTest20220613()
        {
            var qrServerMock = new Mock<IServer>();
            qrServerMock.Setup(s => s.ServerID).Returns(new System.Guid());
            qrServerMock.Setup(s => s.ServerName).Returns("QueryReport");
            qrServerMock.Setup(s => s.Endpoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var qrSessionMock = new Mock<IUdpSession>();
            qrSessionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse("192.168.1.1"), 9999));
            qrSessionMock.Setup(s => s.Server).Returns(qrServerMock.Object);
            qrSessionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Test);

            var qrClient = new QueryReport.Entity.Structure.Client(qrSessionMock.Object);

            var sbServerMock = new Mock<IServer>();
            sbServerMock.Setup(s => s.ServerID).Returns(new System.Guid());
            sbServerMock.Setup(s => s.ServerName).Returns("ServerBrowser");
            sbServerMock.Setup(s => s.Endpoint).Returns(new IPEndPoint(IPAddress.Any, 99));
            var sbSessionMock = new Mock<IUdpSession>();
            sbSessionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse("192.168.1.2"), 9999));
            sbSessionMock.Setup(s => s.Server).Returns(qrServerMock.Object);
            sbSessionMock.Setup(s => s.ConnectionType).Returns(NetworkConnectionType.Test);
            var sbClient = new Client(sbSessionMock.Object);

            // Given
            var qrRequests = new List<byte[]>(){
                new byte[]{0x09,0x00,0x00,0x00,0x00,0x67,0x6D,0x74,0x65,0x73,0x74,0x00},
                new byte[]{0x03,0xB4,0xA3,0xCC,0x80,0x6C,0x6F,0x63,0x61,0x6C,0x69,0x70,0x30,0x00,0x31,0x39,0x32,0x2E,0x31,0x36,0x38,0x2E,0x30,0x2E,0x31,0x30,0x39,0x00,0x6C,0x6F,0x63,0x61,0x6C,0x70,0x6F,0x72,0x74,0x00,0x36,0x35,0x30,0x30,0x00,0x6E,0x61,0x74,0x6E,0x65,0x67,0x00,0x30,0x00,0x73,0x74,0x61,0x74,0x65,0x63,0x68,0x61,0x6E,0x67,0x65,0x64,0x00,0x33,0x00,0x67,0x61,0x6D,0x65,0x6E,0x61,0x6D,0x65,0x00,0x67,0x6D,0x74,0x65,0x73,0x74,0x00,0x70,0x75,0x62,0x6C,0x69,0x63,0x69,0x70,0x00,0x30,0x00,0x70,0x75,0x62,0x6C,0x69,0x63,0x70,0x6F,0x72,0x74,0x00,0x30,0x00,0x68,0x6F,0x73,0x74,0x6E,0x61,0x6D,0x65,0x00,0x4D,0x79,0x20,0x53,0x65,0x72,0x76,0x65,0x72,0x00,0x67,0x61,0x6D,0x65,0x6D,0x6F,0x64,0x65,0x00,0x6F,0x70,0x65,0x6E,0x73,0x74,0x61,0x67,0x69,0x6E,0x67,0x00,0x67,0x72,0x6F,0x75,0x70,0x69,0x64,0x00,0x33,0x00,0x6E,0x75,0x6D,0x70,0x6C,0x61,0x79,0x65,0x72,0x73,0x00,0x31,0x00,0x6D,0x61,0x78,0x70,0x6C,0x61,0x79,0x65,0x72,0x73,0x00,0x38,0x00,0x67,0x61,0x6D,0x65,0x76,0x65,0x72,0x00,0x31,0x2E,0x30,0x31,0x00,0x00,0x00,0x01,0x70,0x6C,0x61,0x79,0x65,0x72,0x5F,0x00,0x70,0x69,0x6E,0x67,0x5F,0x00,0x00,0x50,0x65,0x65,0x72,0x50,0x6C,0x61,0x79,0x65,0x72,0x31,0x00,0x30,0x00,0x00,0x00,0x00},
                new byte[]{0x08,0xB4,0xA3,0xCC,0x80},
                new byte[] {0x03,0xB4,0xA3,0xCC,0x80,0x6C,0x6F,0x63,0x61,0x6C,0x69,0x70,0x30,0x00,0x31,0x39,0x32,0x2E,0x31,0x36,0x38,0x2E,0x30,0x2E,0x31,0x30,0x39,0x00,0x6C,0x6F,0x63,0x61,0x6C,0x70,0x6F,0x72,0x74,0x00,0x36,0x35,0x30,0x30,0x00,0x6E,0x61,0x74,0x6E,0x65,0x67,0x00,0x30,0x00,0x73,0x74,0x61,0x74,0x65,0x63,0x68,0x61,0x6E,0x67,0x65,0x64,0x00,0x32,0x00,0x67,0x61,0x6D,0x65,0x6E,0x61,0x6D,0x65,0x00,0x67,0x6D,0x74,0x65,0x73,0x74,0x00,0x70,0x75,0x62,0x6C,0x69,0x63,0x69,0x70,0x00,0x30,0x00,0x70,0x75,0x62,0x6C,0x69,0x63,0x70,0x6F,0x72,0x74,0x00,0x30,0x00,0x00}
            };
            var sb1Requests = new List<byte[]>()
            {
                new byte[]{0x00,0x5A,0x00,0x01,0x03,0x00,0x00,0x00,0x00,0x67,0x6D,0x74,0x65,0x73,0x74,0x00,0x67,0x6D,0x74,0x65,0x73,0x74,0x00,0x29,0x3E,0x7C,0x23,0x43,0x5D,0x68,0x49,0x00,0x5C,0x68,0x6F,0x73,0x74,0x6E,0x61,0x6D,0x65,0x5C,0x6E,0x75,0x6D,0x77,0x61,0x69,0x74,0x69,0x6E,0x67,0x5C,0x6D,0x61,0x78,0x77,0x61,0x69,0x74,0x69,0x6E,0x67,0x5C,0x6E,0x75,0x6D,0x73,0x65,0x72,0x76,0x65,0x72,0x73,0x5C,0x6E,0x75,0x6D,0x70,0x6C,0x61,0x79,0x65,0x72,0x73,0x00,0x00,0x00,0x00,0x20}
            };
            var sb2Requests = new List<byte[]>()
            {
                new byte[]{0x00,0x5A,0x00,0x01,0x03,0x00,0x00,0x00,0x00,0x67,0x6D,0x74,0x65,0x73,0x74,0x00,0x67,0x6D,0x74,0x65,0x73,0x74,0x00,0x29,0x3E,0x7C,0x23,0x43,0x5D,0x68,0x49,0x00,0x5C,0x68,0x6F,0x73,0x74,0x6E,0x61,0x6D,0x65,0x5C,0x6E,0x75,0x6D,0x77,0x61,0x69,0x74,0x69,0x6E,0x67,0x5C,0x6D,0x61,0x78,0x77,0x61,0x69,0x74,0x69,0x6E,0x67,0x5C,0x6E,0x75,0x6D,0x73,0x65,0x72,0x76,0x65,0x72,0x73,0x5C,0x6E,0x75,0x6D,0x70,0x6C,0x61,0x79,0x65,0x72,0x73,0x00,0x00,0x00,0x00,0x20},
                new byte[]{0x00,0x5A,0x00,0x01,0x03,0x00,0x00,0x00,0x00,0x67,0x6D,0x74,0x65,0x73,0x74,0x00,0x67,0x6D,0x74,0x65,0x73,0x74,0x00,0x24,0x68,0x41,0x78,0x23,0x39,0x59,0x70,0x00,0x5C,0x68,0x6F,0x73,0x74,0x6E,0x61,0x6D,0x65,0x5C,0x6E,0x75,0x6D,0x77,0x61,0x69,0x74,0x69,0x6E,0x67,0x5C,0x6D,0x61,0x78,0x77,0x61,0x69,0x74,0x69,0x6E,0x67,0x5C,0x6E,0x75,0x6D,0x73,0x65,0x72,0x76,0x65,0x72,0x73,0x5C,0x6E,0x75,0x6D,0x70,0x6C,0x61,0x79,0x65,0x72,0x73,0x00,0x00,0x00,0x00,0x20},
                new byte[]{0x00,0x5E,0x00,0x01,0x03,0x00,0x00,0x00,0x00,0x67,0x6D,0x74,0x65,0x73,0x74,0x00,0x67,0x6D,0x74,0x65,0x73,0x74,0x00,0x28,0x76,0x7D,0x30,0x33,0x74,0x35,0x52,0x67,0x72,0x6F,0x75,0x70,0x69,0x64,0x3D,0x33,0x00,0x5C,0x68,0x6F,0x73,0x74,0x6E,0x61,0x6D,0x65,0x5C,0x67,0x61,0x6D,0x65,0x6D,0x6F,0x64,0x65,0x5C,0x67,0x61,0x6D,0x65,0x76,0x65,0x72,0x5C,0x6E,0x75,0x6D,0x70,0x6C,0x61,0x79,0x65,0x72,0x73,0x5C,0x6D,0x61,0x78,0x70,0x6C,0x61,0x79,0x65,0x72,0x73,0x00,0x00,0x00,0x00,0x04},
                new byte[]{0x00,0x5E,0x00,0x01,0x03,0x00,0x00,0x00,0x00,0x67,0x6D,0x74,0x65,0x73,0x74,0x00,0x67,0x6D,0x74,0x65,0x73,0x74,0x00,0x2D,0x42,0x4C,0x67,0x32,0x73,0x28,0x26,0x67,0x72,0x6F,0x75,0x70,0x69,0x64,0x3D,0x33,0x00,0x5C,0x68,0x6F,0x73,0x74,0x6E,0x61,0x6D,0x65,0x5C,0x67,0x61,0x6D,0x65,0x6D,0x6F,0x64,0x65,0x5C,0x67,0x61,0x6D,0x65,0x76,0x65,0x72,0x5C,0x6E,0x75,0x6D,0x70,0x6C,0x61,0x79,0x65,0x72,0x73,0x5C,0x6D,0x61,0x78,0x70,0x6C,0x61,0x79,0x65,0x72,0x73,0x00,0x00,0x00,0x00,0x04}
            };
            foreach (var qrReq in qrRequests)
            {
                new QueryReport.Handler.CmdSwitcher(qrClient, qrReq).Switch();
            }

            foreach (var sbReq in sb2Requests)
            {
                new CmdSwitcher(sbClient, sbReq).Switch();
            }
            // When

            // Then
        }
    }
}