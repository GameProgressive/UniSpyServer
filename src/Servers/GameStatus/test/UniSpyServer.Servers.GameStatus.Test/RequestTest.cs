using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Handler.CmdHandler;
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
    }
}
