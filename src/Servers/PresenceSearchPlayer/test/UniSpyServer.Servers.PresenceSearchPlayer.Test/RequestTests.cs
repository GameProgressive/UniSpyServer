using System;
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
            Assert.Equal(request.Nick, "xiaojiuwo");
            Assert.Equal(request.Email, "xiaojiuwo@gamespy.com");
            // password 'xxx' is decoded and hash to '1e034b66363e5a081874ae022767f685'
            Assert.Equal(request.Password, "1e034b66363e5a081874ae022767f685");
            Assert.Equal(request.ProductID, (uint)0);
            Assert.Equal(request.NamespaceID, (uint)0);
            Assert.Equal(request.Uniquenick, "xiaojiuwo");
            Assert.Equal(request.CDKey, "xxx-xxx-xxx-xxx");
            Assert.Equal(request.PartnerID, (uint)0);
            Assert.Equal(request.GameName, "gmtest");
        }
    }
}
