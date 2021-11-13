using Xunit;
using UniSpyServer.Servers.WebServer.Entity.Structure.Request.RacingRequest;

namespace UniSpyServer.Servers.WebServer.Test.Racing
{
    public class RequestsTest
    {
        //
        // These are the SOAP requests of RACE
        // Endpoint: {FQDN}/RaceService/
        //
        [Fact]
        public void GetContestData()
        {
            var request = new GetContestDataRequest(RawRequests.GetContestData);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.RegionId.ToString());
            Assert.Equal("0", request.CourseId.ToString());
        }
        [Fact]
        public void GetFriendRankings()
        {
            var request = new GetFriendRankingsRequest(RawRequests.GetFriendRankings);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.RegionId.ToString());
            Assert.Equal("0", request.CourseId.ToString());
            Assert.Equal("0", request.ProfileId.ToString());
        }
        [Fact]
        public void GetRegionalData()
        {
            var request = new GetRegionalDataRequest(RawRequests.GetRegionalData);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.RegionId.ToString());
        }
        [Fact]
        public void GetTenAboveRankings()
        {
            var request = new GetTenAboveRankingsRequest(RawRequests.GetTenAboveRankings);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.RegionId.ToString());
            Assert.Equal("0", request.CourseId.ToString());
            Assert.Equal("0", request.ProfileId.ToString());
        }
        [Fact]
        public void GetTopTenRankings()
        {
            var request = new GetTopTenRankingsRequest(RawRequests.GetTopTenRankings);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.RegionId.ToString());
            Assert.Equal("0", request.CourseId.ToString());
        }
        [Fact]
        public void SubmitGhost()
        {
            var request = new SubmitGhostRequest(RawRequests.SubmitGhost);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.RegionId.ToString());
            Assert.Equal("0", request.CourseId.ToString());
            Assert.Equal("0", request.ProfileId.ToString());
            Assert.Equal("XXXXXX", request.Score);
            Assert.Equal("0", request.FileId.ToString());
        }
        [Fact]
        public void SubmitScores()
        {
            var request = new SubmitScoresRequest(RawRequests.SubmitScores);
            request.Parse();
            Assert.Equal("0", request.GameData.ToString());
            Assert.Equal("0", request.RegionId.ToString());
            Assert.Equal("0", request.ProfileId.ToString());
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.ScoreMode.ToString());
            Assert.Equal("XXXXXX", request.ScoreDatas);
        }
    }
}
