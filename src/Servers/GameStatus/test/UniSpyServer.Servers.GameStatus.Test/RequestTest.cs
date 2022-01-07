using UniSpyServer.Servers.GameStatus.Entity.Enumerate;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using Xunit;

namespace UniSpyServer.Servers.GameStatus.Test
{
    public class RequestTest
    {
        [Fact]
        public void AuthPlayerRequestTest()
        {
            var raw = @"\auth\\gamename\crysis2\response\xxxxx\port\30\id\1\final";
            var request = new AuthGameRequest(raw);
            request.Parse();
            Assert.Equal("crysis2", request.GameName);
            Assert.Equal(30, request.Port);
            Assert.Equal((uint)1, request.OperationID);
        }
        [Fact]
        public void AuthGameRequestTest()
        {
            var raw = @"\auth\\gamename\gmtest\response\xxxx\port\0\id\1";
            var request = new AuthGameRequest(raw);
            request.Parse();
            Assert.Equal("gmtest", request.GameName);
            Assert.Equal(0, request.Port);
            Assert.Equal((uint)1, request.OperationID);
        }
        [Fact]
        public void GetPlayerDataRequestTest()
        {
            var raw = @"\getpd\\pid\0\ptype\0\dindex\1\keys\hello" + "\x1" + @"hi\lid\1";
            var request = new GetPlayerDataRequest(raw);
            request.Parse();
            Assert.Equal((uint)0, request.ProfileID);
            Assert.Equal(PersistStorageType.PrivateRO, request.StorageType);
            Assert.Equal((uint)1, request.DataIndex);
            Assert.Equal(2, request.Keys.Count);
            Assert.Equal("hello", request.Keys[0]);
            Assert.Equal("hi", request.Keys[1]);
        }
        [Fact]
        public void GetProfileIDRequestTest()
        {
            var raw = @"\getpid\\nick\xiaojiuwo\keyhash\00000\lid\1";
            var request = new GetProfileIDRequest(raw);
            request.Parse();
            Assert.Equal("xiaojiuwo", request.Nick);
            Assert.Equal("00000", request.KeyHash);
            Assert.Equal((uint)1, request.OperationID);
        }
        [Fact(Skip = "Not implemented yet.")]
        public void NewGameRequestTest()
        {
            // Given

            // When

            // Then
        }
    }
}
