using UniSpy.Server.Core.Encryption;
using UniSpy.Server.ServerBrowser.V1.Contract.Request;
using Xunit;

namespace UniSpy.Server.ServerBrowser.Test.V1
{
    public class RequestTest
    {
        [Fact]
        public void Armada220230413()
        {
            var raw1 = @"\gamename\armada2\gamever\1.6\location\0\validate\Qsu/4AdL\final\\queryid\1.1\";
            var request1 = new GameNameRequest(raw1);
            request1.Parse();
            Assert.True(request1.GameName == "armada2");
            Assert.True(request1.Version == "1.6");
            var raw2 = @"\list\groups\gamename\armada2\final\";
            var request2 = new ListRequest(raw2);
            request2.Parse();
            Assert.True(request2.Type == ListRequestType.Group);
            Assert.True(request2.GameName == "armada2");
        }
    }
}