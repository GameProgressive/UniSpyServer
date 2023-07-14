using System.Collections.Generic;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;
using Xunit;

namespace UniSpy.Server.PresenceConnectionManager.Test
{
    public class GameTest
    {
        [Fact]
        public void Civilization4()
        {
            var rawRequests = new List<string>()
            {
                @"\newuser\\email\civ4@unispy.org\nick\civ4-tk\passwordenc\JMHGwQ__\productid\10435\gamename\civ4\namespaceid\17\uniquenick\civ4-tk\id\1\final\",
                @"\login\\challenge\xMsHUXuWNXL3KMwmhoQZJrP0RVsArCYT\uniquenick\civ4-tk\userid\25\profileid\26\response\7f2c9c6685570ea18b7207d2cbd72452\firewall\1\port\0\productid\10435\gamename\civ4\namespaceid\17\sdkrevision\1\id\1\final\"
            };
            foreach (var raw in rawRequests)
            {
                ((ITestClient)MokeObject.client).TestReceived(UniSpyEncoding.GetBytes(raw));
            }
        }
        [Fact]
        public void ConflictGlobalStorm()
        {
            var rawRequests = new List<string>()
            {
                @"\lc\1\challenge\NRNUJLZMLX\id\1\final\",
                @"\login\\challenge\KMylyQbZfqzKn9otxx32q4076sOUnKif\user\cgs1@cgs1@rs.de\response\c1a6638bbcfe130e4287bfe4aa792949\port\-15737\productid\10469\gamename\conflictsopc\namespaceid\1\id\1\final\",
                @"\inviteto\\sesskey\58366\products\1038\final\"
            };
            foreach (var raw in rawRequests)
            {
                ((ITestClient)MokeObject.client).TestReceived(UniSpyEncoding.GetBytes(raw));
            }
        }
        [Fact]
        public void swbfrontps2Test()
        {
            var raw = @"\status\1\sesskey\1111\statstring\EN LIGNE\locstring\\final\";
            var req = new StatusRequest(raw);
            req.Parse();
            Assert.True(req.Status.LocationString == "");
            Assert.True(req.Status.StatusString == "EN LIGNE");
        }
    }
}