using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using Xunit;

namespace UniSpyServer.Servers.NatNegotiation.Test
{
    public class RequestTest
    {
        [Fact]
        public void InitTest()
        {
            var rawRequest = new byte[] {
            0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2, 0x03,
            0x00,
            0x00, 0x00, 0x03, 0x09, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var request = new InitRequest(rawRequest);
            request.Parse();
            Assert.Equal(RequestType.Init, request.CommandName);
            Assert.Equal((uint)151191552, request.Cookie);
            Assert.Equal(NatClientIndex.GameClient, request.ClientIndex);
            Assert.Equal((byte)0, request.UseGamePort);
            Assert.Equal((byte)3, request.Version);
            Assert.Equal((byte)0, request.UseGamePort);
            Assert.Equal(NatPortType.NN1, request.PortType);
        }
        [Fact]
        public void AddressTest()
        {
            var rawRequest = new byte[] { 0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2, 0x03, 0x0a, 0x00, 0x00, 0x03, 0x09, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var request = new AddressCheckRequest(rawRequest);
            request.Parse();
            Assert.Equal(RequestType.AddressCheck, request.CommandName);
            Assert.Equal((uint)151191552, request.Cookie);
            Assert.Equal(NatClientIndex.GameClient, request.ClientIndex);
            Assert.Equal((byte)0, request.UseGamePort);
            Assert.Equal((byte)3, request.Version);
            Assert.Equal(NatPortType.NN1, request.PortType);
        }
        [Fact]
        public void ErtAckTest()
        {
            var rawRequest = new byte[] {
            0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2, 0x03,
            0x03,
            0x00, 0x00, 0x03, 0x09,
            0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var request = new ErtAckRequest(rawRequest);
            request.Parse();
            Assert.Equal(RequestType.ErtAck, request.CommandName);
            Assert.Equal((uint)151191552, request.Cookie);
            Assert.Equal(NatClientIndex.GameClient, request.ClientIndex);
            Assert.Equal((byte)0, request.UseGamePort);
            Assert.Equal((byte)3, request.Version);
            Assert.Equal((byte)0, request.UseGamePort);
            Assert.Equal(NatPortType.NN1, request.PortType);
        }
        [Fact]
        public void NatifyTest()
        {
            var rawRequest = new byte[] {
            0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2, 0x03,
            0x0c,
            0x00, 0x00, 0x03, 0x09,
            0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var request = new ErtAckRequest(rawRequest);
            request.Parse();
            Assert.Equal(RequestType.NatifyRequest, request.CommandName);
            Assert.Equal((uint)151191552, request.Cookie);
            Assert.Equal(NatClientIndex.GameClient, request.ClientIndex);
            Assert.Equal((byte)0, request.UseGamePort);
            Assert.Equal((byte)3, request.Version);
            Assert.Equal((byte)0, request.UseGamePort);
            Assert.Equal(NatPortType.NN1, request.PortType);
        }
        [Fact(Skip = "Not implemented")]
        public void ReportTest()
        {

        }
        [Fact]
        public void PreInitTest()
        {
            var raw = new byte[] { 0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2, 0x04, 0x0f, 0xb5, 0xe0, 0x95, 0x2a, 0x00, 0x24, 0x38, 0xb2, 0xb3, 0x5e };
            var request = new PreInitRequest(raw);
            request.Parse();
            Assert.Equal(RequestType.PreInit, request.CommandName);
            Assert.Equal(PreInitState.WaitingForClient, request.State);
        }
    }
}
