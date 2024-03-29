using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Result
{
    public sealed class GetProfileDataModel
    {
        public string Nick;
        public int ProfileId;
        public string UniqueNick;
        public string Email;
        public string Firstname;
        public string Lastname;
        public int? Icquin;
        public string Homepage;
        public string Zipcode;
        public string Countrycode;
        public double? Longitude;
        public double? Latitude;
        public string Location;
        public int? Birthday;
        public int? Birthmonth;
        public int? Birthyear;
        public byte? Sex;
        public int? Publicmask;
        public string Aim;
        public int? Picture;
        public int? Occupationid;
        public int? Industryid;
        public int? Incomeid;
        public int? Marriedid;
        public int? Childcount;
        public int? Interests1;
        public int? Ownership1;
        public int? Connectiontype;

    }
    public sealed class GetProfileResult : ResultBase
    {
        public GetProfileDataModel UserProfile;
        public GetProfileResult()
        {
        }
    }
}
