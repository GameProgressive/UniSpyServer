using Xunit;
using UniSpyServer.WebServer.Entity.Structure.Request.AtlasRequest;
using UniSpyServer.Servers.WebServer.Test.Request;

namespace UniSpyServer.Servers.UniSpyServer.WebServer.Test.RequestTest
{
    public class AtlasRequestsTest
    {
        //
        // These are the SOAP requests of ATLAS (Competition)
        // Endpoint: {FQDN}/competition/
        //
        [Fact]
        public void CreateMatchlessSession()
        {
            var request = new CreateMatchlessSessionRequest(AtlasRequests.CreateMatchlessSession);
            request.Parse();
            Assert.Equal("XXXXXX", request.Certificate);
            Assert.Equal("XXXXXX", request.Proof);
            Assert.Equal("0", request.GameId.ToString());
        }
        [Fact]
        public void CreateSession()
        {
            var request = new CreateSessionRequest(AtlasRequests.CreateSession);
            request.Parse();
            Assert.Equal("XXXXXX", request.Certificate);
            Assert.Equal("XXXXXX", request.Proof);
            Assert.Equal("0", request.GameId.ToString());
        }
        [Fact]
        public void SetReportIntention()
        {
            var request = new SetReportIntentionRequest(AtlasRequests.SetReportIntention);
            request.Parse();
            Assert.Equal("XXXXXX", request.Certificate);
            Assert.Equal("XXXXXX", request.Proof);
            Assert.Equal("0", request.CsId.ToString());
            Assert.Equal("0", request.CcId.ToString());
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.Authoritative);
        }
        [Fact]
        public void SubmitReport()
        {
            var request = new SubmitReportRequest(AtlasRequests.SubmitReport);
            request.Parse();
            Assert.Equal("XXXXXX", request.Certificate);
            Assert.Equal("XXXXXX", request.Proof);
            Assert.Equal("0", request.CsId.ToString());
            Assert.Equal("0", request.CcId.ToString());
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.Authoritative);
        }
    }
}
