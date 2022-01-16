using Xunit;
using UniSpyServer.Servers.WebServer.Handler.CmdHandler.Sake;
using UniSpyServer.Servers.WebServer.Entity.Structure.Request.Sake;

namespace UniSpyServer.Servers.WebServer.Test.Sake
{
    public class HandlerTest
    {
        [Fact]
        public void CreateRecordTest()
        {
            var request = new CreateRecordRequest(RawRequests.CreateRecord);
            var handler = new CreateRecordHandler(null, request);
            handler.Handle();
        }
    }
}