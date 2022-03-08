using System.Linq;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("updatepro")]
    public sealed class UpdateProHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        private new UpdateProRequest _request => (UpdateProRequest)base._request;
        public UpdateProHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            using (var db = new UniSpyContext())
            {
                Profile profile = db.Profiles.Where(
                    p => p.Userid == _client.Info.UserInfo.UserId
                    && p.ProfileId == _client.Info.ProfileInfo.ProfileId
                    && p.Nick == p.Nick).First();

                User user = db.Users.Where(
                    u => u.UserId == _client.Info.UserInfo.UserId).First();

                Subprofile subprofile = db.Subprofiles.Where(
                    s => s.ProfileId == _client.Info.ProfileInfo.ProfileId
                    && s.NamespaceId == _client.Info.SubProfileInfo.NamespaceId
                    && s.Uniquenick == _client.Info.SubProfileInfo.Uniquenick).First();

                if (_request.HasPublicMaskFlag)
                {
                    profile.Publicmask = (int)_request.PublicMask;
                }
                if (_request.HasFirstNameFlag)
                {
                    profile.Firstname = _request.FirstName;
                }
                if (_request.HasLastNameFlag)
                {
                    profile.Lastname = _request.LastName;
                }
                if (_request.HasICQFlag)
                {
                    profile.Icquin = _request.ICQUIN;
                }
                if (_request.HasHomePageFlag)
                {
                    profile.Homepage = _request.HomePage;
                }
                if (_request.HasBirthdayFlag)
                {
                    profile.Birthday = _request.BirthDay;
                    profile.Birthmonth = _request.BirthMonth;
                    profile.Birthyear = _request.BirthYear;
                }
                if (_request.HasSexFlag)
                {
                    profile.Sex = _request.Sex;
                }

                if (_request.HasZipCode)
                {
                    profile.Zipcode = _request.ZipCode;
                }
                if (_request.HasCountryCode)
                {
                    profile.Countrycode = _request.CountryCode;
                }

                db.Update(profile);
                // we update the profile in our memory
                _client.Info.ProfileInfo = profile;
            }
        }
    }
}

