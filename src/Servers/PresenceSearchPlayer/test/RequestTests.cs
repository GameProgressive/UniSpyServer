using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Request;
using Xunit;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Test
{
    public class RequestTests
    {
        [Fact]
        public void NewUserTest()
        {
            var request = new NewUserRequest(RawRequests.NewUser);
            request.Parse();
            Assert.Equal("xiaojiuwo", request.Nick);
            Assert.Equal("xiaojiuwo@gamespy.com", request.Email);
            // password 'xxx' is decoded and hash to '1e034b66363e5a081874ae022767f685'
            Assert.Equal("0d1b08c34858921bc7c662b228acb7ba", request.Password);
            Assert.Equal((int)0, request.ProductID);
            Assert.Equal((int)0, request.NamespaceID);
            Assert.Equal("xiaojiuwo", request.Uniquenick);
            Assert.Equal("xxx-xxx-xxx-xxx", request.CDKey);
            Assert.Equal((int)0, request.PartnerID);
            Assert.Equal("gmtest", request.GameName);
        }
    }
}
