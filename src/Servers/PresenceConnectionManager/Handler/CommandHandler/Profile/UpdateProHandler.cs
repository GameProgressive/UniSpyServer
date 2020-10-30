using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Entity.Structure.Request.Profile;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.CommandHandler.Profile
{
    public class UpdateProHandler : PCMCommandHandlerBase
    {
        protected UpdateProRequest _request;
        public UpdateProHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
            _request = new UpdateProRequest(recv);
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                Profiles profile = db.Profiles.Where(
                    p => p.Userid == _session.UserData.UserID
                    && p.Profileid == _session.UserData.ProfileID
                    && p.Nick == p.Nick).First();

                Users user = db.Users.Where(
                    u => u.Userid == _session.UserData.UserID).First();

                Subprofiles subprofile = db.Subprofiles.Where(
                    s => s.Profileid == _session.UserData.ProfileID
                    && s.Namespaceid == _session.UserData.NamespaceID
                    && s.Uniquenick == _session.UserData.UniqueNick).First();

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

