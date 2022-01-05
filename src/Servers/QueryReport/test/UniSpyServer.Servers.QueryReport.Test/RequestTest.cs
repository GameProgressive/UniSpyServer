using System;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Request;
using Xunit;
namespace UniSpyServer.Servers.UniSpyServer.Servers.QueryReport.Test
{
    public class RequestTest
    {
        [Fact]
        public void AvaliableRequestTest()
        {
            var rawRequest = new byte[]{
                0x09,//packet type
                0x00,0x00,0x00,0x00,//instant key
                0x09, 0x00, 0x00, 0x00, 0x00,//prefix
                0x67, 0x61, 0x6D, 0x65 ,0x73, 0x70, 0x79,//gamename
                0x00
            };
            var request = new AvaliableRequest(rawRequest);
            request.Parse();
            Assert.Equal(PacketType.AvaliableCheck, request.CommandName);
            Assert.Equal((uint)0, request.InstantKey);
        }
        [Fact]
        public void ChallengeRequestTest()
        {
            var rawRequest = new byte[]{
                0x01,//packet type
                0x00,0x00,0x00,0x00,//instant key
                0x67, 0x61, 0x6D, 0x65 ,0x73, 0x70, 0x79,//gamename
                0x00
            };
            var request = new ChallengeRequest(rawRequest);
            request.Parse();
            Assert.Equal(PacketType.Challenge, request.CommandName);
            Assert.Equal((uint)0, request.InstantKey);
        }
        [Fact]
        public void EchoRequest()
        {
            var rawRequest = new byte[]{
                0x02,//packet type
                0x00,0x00,0x00,0x00,//instant key
                0x67, 0x61, 0x6D, 0x65 ,0x73, 0x70, 0x79,//gamename
                0x00
            };
            var request = new EchoRequest(rawRequest);
            request.Parse();
            Assert.Equal(PacketType.Echo, request.CommandName);
            Assert.Equal((uint)0, request.InstantKey);
        }
        [Fact(Skip = "Not implemented")]
        public void HeartBeatRequestTest()
        {
            // Given

            // When

            // Then
        }
        [Fact]
        public void KeepAliveRequestTest()
        {
            var rawRequest = new byte[]{
                0x08,//packet type
                0x00,0x00,0x00,0x00,//instant key
            };
            var request = new EchoRequest(rawRequest);
            request.Parse();
            Assert.Equal(PacketType.KeepAlive, request.CommandName);
            Assert.Equal((uint)0, request.InstantKey);
        }
    }
}
