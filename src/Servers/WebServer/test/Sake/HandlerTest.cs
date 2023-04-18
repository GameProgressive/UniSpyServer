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
            var client = MokeObject.CreateClient();
            var request = new CreateRecordRequest(RawRequests.CreateRecord);
            var handler = new CreateRecordHandler(client, request);
            Assert.Throws<System.NotImplementedException>(() => handler.Handle());
        }
    }
}