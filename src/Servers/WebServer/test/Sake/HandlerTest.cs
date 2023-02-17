using UniSpy.Server.WebServer.Module.Sake.Handler;
using UniSpy.Server.WebServer.Module.Sake.Contract.Request;
using Xunit;

namespace UniSpy.Server.WebServer.Test.Sake
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