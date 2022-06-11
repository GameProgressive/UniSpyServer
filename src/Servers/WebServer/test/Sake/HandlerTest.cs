using UniSpyServer.Servers.WebServer.Module.Sake.Handler;
using UniSpyServer.Servers.WebServer.Module.Sake.Structure.Request;
using Xunit;

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