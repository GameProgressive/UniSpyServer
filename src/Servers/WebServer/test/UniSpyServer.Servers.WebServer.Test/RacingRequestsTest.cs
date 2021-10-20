using System;
using Xunit;
using UniSpyServer.WebServer.Entity.Structure.Request.RacingRequest;
namespace UniSpyServer.Servers.UniSpyServer.WebServer.RequestTest
{
    public class RacingRequestsTest
    {
        //
        // These are the SOAP requests of RACE
        // Endpoint: {FQDN}/RaceService/
        //
        [Fact]
        public void GetContestData()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:GetContestData>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:regionid>0</ns1:regionid>
                            <ns1:courseid>0</ns1:courseid>
                        </ns1:GetContestData>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new GetContestDataRequest(rawRequest);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.RegionId.ToString());
            Assert.Equal("0", request.CourseId.ToString());
        }
        [Fact]
        public void GetFriendRankings()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:GetFriendRankings>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:regionid>0</ns1:regionid>
                            <ns1:courseid>0</ns1:courseid>
                            <ns1:profileid>0</ns1:profileid>
                        </ns1:GetFriendRankings>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new GetFriendRankingsRequest(rawRequest);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.RegionId.ToString());
            Assert.Equal("0", request.CourseId.ToString());
            Assert.Equal("0", request.ProfileId.ToString());
        }
        [Fact]
        public void GetRegionalData()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:GetRegionalData>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:regionid>0</ns1:regionid>
                        </ns1:GetRegionalData>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new GetRegionalDataRequest(rawRequest);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.RegionId.ToString());
        }
        [Fact]
        public void GetTenAboveRankings()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:GetTenAboveRankings>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:regionid>0</ns1:regionid>
                            <ns1:courseid>0</ns1:courseid>
                            <ns1:profileid>0</ns1:profileid>
                        </ns1:GetTenAboveRankings>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new GetTenAboveRankingsRequest(rawRequest);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.RegionId.ToString());
            Assert.Equal("0", request.CourseId.ToString());
            Assert.Equal("0", request.ProfileId.ToString());
        }
        [Fact]
        public void GetTopTenRankings()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:GetTopTenRankings>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:regionid>0</ns1:regionid>
                            <ns1:courseid>0</ns1:courseid>
                        </ns1:GetTopTenRankings>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new GetTopTenRankingsRequest(rawRequest);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.RegionId.ToString());
            Assert.Equal("0", request.CourseId.ToString());
        }
        [Fact]
        public void SubmitGhost()
        {
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:SubmitGhost>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:regionid>0</ns1:regionid>
                            <ns1:courseid>0</ns1:courseid>
                            <ns1:profileid>0</ns1:profileid>
                            <ns1:score>XXXXXX</ns1:score>
                            <ns1:fileid>0</ns1:fileid>
                        </ns1:SubmitGhost>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new SubmitGhostRequest(rawRequest);
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
            var rawRequest =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:ns1=""http://gamespy.net/sake"">
                    <SOAP-ENV:Body>
                        <ns1:SubmitScores>
                            <ns1:gamedata>0</ns1:gamedata>
                            <ns1:regionid>0</ns1:regionid>
                            <ns1:profileid>0</ns1:profileid>
                            <ns1:gameid>0</ns1:gameid>
                            <ns1:scoremode>0</ns1:scoremode>
                            <ns1:scoredatas>XXXXXX</ns1:scoredatas>
                        </ns1:SubmitScores>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

            var request = new SubmitScoresRequest(rawRequest);
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
