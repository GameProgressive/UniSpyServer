using UniSpy.Server.PresenceConnectionManager.Application;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Profile
{

    public sealed class UpdateProfileHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        private new UpdateProfileRequest _request => (UpdateProfileRequest)base._request;
        public UpdateProfileHandler(Client client, UpdateProfileRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            var profile = _client.Info.ProfileInfo;

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

            StorageOperation.Persistance.UpdateProfileInfo(profile);
        }
    }
}

