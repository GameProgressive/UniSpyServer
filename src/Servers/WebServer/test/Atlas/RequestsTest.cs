using Xunit;
using UniSpyServer.Servers.WebServer.Entity.Structure.Request.Atlas;

namespace UniSpyServer.Servers.WebServer.Test.Atlas
{
    public class RequestsTest
    {
        //
        // These are the SOAP requests of ATLAS (Competition)
        // Endpoint: {FQDN}/competition/
        //
        [Fact]
        public void CreateMatchlessSession()
        {
            var request = new CreateMatchlessSessionRequest(RawRequests.CreateMatchlessSession);
            request.Parse();
            Assert.Equal("XXXXXX", request.Certificate);
            Assert.Equal("XXXXXX", request.Proof);
            Assert.Equal("0", request.GameId.ToString());
        }
        [Fact]
        public void CreateSession()
        {
            var request = new CreateSessionRequest(RawRequests.CreateSession);
            request.Parse();
            Assert.Equal("XXXXXX", request.Certificate);
            Assert.Equal("XXXXXX", request.Proof);
            Assert.Equal("0", request.GameId.ToString());
        }
        [Fact]
        public void SetReportIntention()
        {
            var request = new SetReportIntentionRequest(RawRequests.SetReportIntention);
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
            var request = new SubmitReportRequest(RawRequests.SubmitReport);
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
