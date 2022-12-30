using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using Xunit;

namespace UniSpyServer.Servers.PresenceConnectionManager.Test
{
    public class ProfileRequestTest
    {
        [Fact]
        public void AddBlock()
        {
            var request = new AddBlockRequest(ProfileRequests.AddBlock);
            request.Parse();
            Assert.Equal((int)0, request.TargetId);
        }

        [Fact]
        public void GetProfile()
        {
            var request = new GetProfileRequest(ProfileRequests.GetProfile);
            request.Parse();
            Assert.Equal((int)0, request.ProfileId);
            Assert.Equal("xxxx", request.SessionKey);
        }

        [Fact]
        public void NewProfile()
        {
            var request = new NewProfileRequest(ProfileRequests.NewProfile);
            request.Parse();
            Assert.Equal("xxxx", request.SessionKey);
            Assert.Equal("spyguy", request.NewNick);
        }

        [Fact]
        public void NewProfileReplace()
        {
            var request = new NewProfileRequest(ProfileRequests.NewProfileReplace);
            request.Parse();
            Assert.Equal("xxxx", request.SessionKey);
            Assert.Equal("spyguy2", request.NewNick);
            Assert.Equal("spyguy", request.OldNick);
        }

        [Fact]
        public void RegisterCDKey()
        {
            var request = new RegisterCDKeyRequest(ProfileRequests.RegisterCDKey);
            request.Parse();
            Assert.Equal("xxxx", request.SessionKey);
            Assert.Equal("xxxx", request.CDKeyEnc);
        }

        [Fact]
        public void RegisterNick()
        {
            var request = new RegisterNickRequest(ProfileRequests.RegisterNick);
            request.Parse();
            Assert.Equal("xxxx", request.SessionKey);
            Assert.Equal("spyguy", request.UniqueNick);
            Assert.Equal((int)0, request.PartnerId);
        }

        //TODO: publicmasks doesn't work
        /*[Fact]
        public void UpdatePro()
        {
            var request = new UpdateProRequest(ProfileRequests.UpdatePro);
            request.Parse();
            Assert.Equal(PublicMasks.All, request.PublicMask);
            Assert.Equal("xxxx", request.SessionKey);
            Assert.Equal("Spy", request.FirstName);
            Assert.Equal("Guy", request.LastName);
            Assert.Equal((int)0, request.ICQUIN);
            Assert.Equal("unispy.org", request.HomePage);
            Assert.Equal("00000", request.ZipCode);
            Assert.Equal("US", request.CountryCode);
            Assert.Equal(20, request.BirthDay);
            Assert.Equal(3, request.BirthMonth);
            Assert.Equal(1980, request.BirthYear);
            Assert.Equal(0, request.Sex);
            Assert.Equal((int)0, request.PartnerID);
            Assert.Equal("spyguy", request.Nick);
            Assert.Equal("spyguy", request.Uniquenick);
        }*/

        //TODO: not implemented
        /*[Fact]
        public void UpdateUi()
        {
            var request = new UpdateUiRequest(ProfileRequests.UpdateUi);
            request.Parse();
            Assert.Equal("xxxx", request.SessionKey);
            Assert.Equal("0000", request.Password);
            Assert.Equal("spyguy@unispy.org", request.Email);
        }*/
    }
}
