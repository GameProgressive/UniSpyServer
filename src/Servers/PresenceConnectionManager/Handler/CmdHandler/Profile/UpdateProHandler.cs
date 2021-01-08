using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Entity.Structure.Request.Profile;
using System.Collections.Generic;
using System.Linq;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    public class UpdateProHandler : PCMCmdHandlerBase
    {
        protected new UpdateProRequest _request { get { return (UpdateProRequest)base._request; } }
        public UpdateProHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                Profiles profile = db.Profiles.Where(
                    p => p.Userid == _session.UserInfo.UserID
                    && p.Profileid == _session.UserInfo.ProfileID
                    && p.Nick == p.Nick).First();

                Users user = db.Users.Where(
                    u => u.Userid == _session.UserInfo.UserID).First();

                Subprofiles subprofile = db.Subprofiles.Where(
                    s => s.Profileid == _session.UserInfo.ProfileID
                    && s.Namespaceid == _session.UserInfo.NamespaceID
                    && s.Uniquenick == _session.UserInfo.UniqueNick).First();

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

