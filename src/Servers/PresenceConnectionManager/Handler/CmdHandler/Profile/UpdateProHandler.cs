using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Request.Profile;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    [Command("updatepro")]
    internal sealed class UpdateProHandler : PCMCmdHandlerBase
    {
        private new UpdateProRequest _request => (UpdateProRequest)base._request;
        public UpdateProHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            using (var db = new unispyContext())
            {
                Profiles profile = db.Profiles.Where(
                    p => p.Userid == _session.UserInfo.BasicInfo.UserID
                    && p.Profileid == _session.UserInfo.BasicInfo.ProfileID
                    && p.Nick == p.Nick).First();

                Users user = db.Users.Where(
                    u => u.Userid == _session.UserInfo.BasicInfo.UserID).First();

                Subprofiles subprofile = db.Subprofiles.Where(
                    s => s.Profileid == _session.UserInfo.BasicInfo.ProfileID
                    && s.Namespaceid == _session.UserInfo.BasicInfo.NamespaceID
                    && s.Uniquenick == _session.UserInfo.BasicInfo.UniqueNick).First();

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
            }
        }
    }
}

