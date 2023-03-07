using Xunit;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;

namespace UniSpy.Server.PresenceConnectionManager.Test
{
    public class BuddyRequestTest
    {
        [Fact]
        public void AddBuddy()
        {
            var request = new AddBuddyRequest(BuddyRequests.AddBuddy);
            request.Parse();
            Assert.Equal((int)0, request.FriendProfileID);
            Assert.Equal("test", request.Reason);
        }

        [Fact]
        public void DelBuddy()
        {
            var request = new DelBuddyRequest(BuddyRequests.DelBuddy);
            request.Parse();
            Assert.Equal((int)0, request.TargetId);
        }

        [Fact]
        public void InviteTo()
        {
            var request = new InviteToRequest(BuddyRequests.InviteTo);
            request.Parse();
            Assert.Equal((int)0, request.ProductId);
            Assert.Equal((int)0, request.ProfileId);
        }

        // TODO:
        /*[Fact]
        public void StatusInfo()
        {
            var request = new StatusInfoRequest(BuddyRequests.StatusInfo);
            request.Parse();
            Assert.Equal((int)0, request.ProfileId);
            Assert.Equal((int)0, request.NamespaceID);
        }*/

        [Fact]
        public void Status()
        {
            var request = new StatusRequest(BuddyRequests.Status);
            request.Parse();
            Assert.Equal("test", request.Status.StatusString);
            Assert.Equal("test", request.Status.LocationString);
        }
    }
}
