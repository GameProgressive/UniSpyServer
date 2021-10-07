using System;
using Xunit;

namespace UniSpyServer.Test.GameStatus
{
    public class RequestDeserializationTest
    {
        [Fact]
        public void AuthPlayerRequestTest()
        {
            var raw = @"\auth\gamename\%s\response\%s\port\%d\id\1\";
            var request = new AuthPlayerRequest(raw);
            request.Parse();
        }
    }
}
