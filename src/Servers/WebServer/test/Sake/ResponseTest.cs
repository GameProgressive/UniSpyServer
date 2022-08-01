using UniSpyServer.Servers.WebServer.Module.Sake.Structure.Request;
using UniSpyServer.Servers.WebServer.Module.Sake.Structure.Response;
using UniSpyServer.Servers.WebServer.Module.Sake.Structure.Result;
using Xunit;

namespace UniSpyServer.Servers.WebServer.Test.Sake
{
    public class ResponseTest
    {
        [Fact]
        public void CreateRecordTest()
        {
            var request = new CreateRecordRequest(RawRequests.CreateRecord);
            request.Parse();
            var result = new CreateRecordResult()
            {
                RecordID = "0",
                TableID = "0",
            };
            result.Fields.Add("hello0");
            result.Fields.Add("hello1");
            result.Fields.Add("hello2");
            var response = new CreateRecordResponse(request, result);
            response.Build();
        }
    }
}
