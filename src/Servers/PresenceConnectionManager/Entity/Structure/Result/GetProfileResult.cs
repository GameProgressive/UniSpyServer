using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Result
{
    internal class GetProfileDataModel
    {
        public string Nick;
        public uint ProfileID;
        public string UniqueNick;
        public string Email;
        public string Firstname;
        public string Lastname;
        public uint? Icquin;
        public string Homepage;
        public string Zipcode;
        public string Countrycode;
        public float Longitude;
        public float Latitude;
        public string Location;
        public int? Birthday;
        public int? Birthmonth;
        public int? Birthyear;
        public byte? Sex;
        public int Publicmask;
        public string Aim;
        public int Picture;
        public int? Occupationid;
        public int? Industryid;
        public int? Incomeid;
        public int? Marriedid;
        public int? Childcount;
        public int? Interests1;
        public int? Ownership1;
        public int? Connectiontype;

    }
    internal class GetProfileResult : PCMResultBase
    {
        public GetProfileDataModel UserProfile;
        public GetProfileResult()
        {
        }
    }
}
