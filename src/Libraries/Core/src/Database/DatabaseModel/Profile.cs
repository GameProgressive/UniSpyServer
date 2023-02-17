using System.Collections.Generic;

namespace UniSpy.Server.Core.Database.DatabaseModel
{
    /// <summary>
    /// User profiles.
    /// </summary>
    public partial class Profile
    {
        public Profile()
        {
            Addrequests = new HashSet<Addrequest>();
            Blockeds = new HashSet<Blocked>();
            Friends = new HashSet<Friend>();
            Pstorages = new HashSet<Pstorage>();
            Subprofiles = new HashSet<Subprofile>();
        }

        public int ProfileId { get; set; }
        public int Userid { get; set; }
        public string Nick { get; set; } = null!;
        public int Serverflag { get; set; }
        public short? Status { get; set; }
        public string Statstring { get; set; }
        public string Location { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int? Publicmask { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Aim { get; set; }
        public int? Picture { get; set; }
        public int? Occupationid { get; set; }
        public int? Incomeid { get; set; }
        public int? Industryid { get; set; }
        public int? Marriedid { get; set; }
        public int? Childcount { get; set; }
        public int? Interests1 { get; set; }
        public int? Ownership1 { get; set; }
        public int? Connectiontype { get; set; }
        public short? Sex { get; set; }
        public string Zipcode { get; set; }
        public string Countrycode { get; set; }
        public string Homepage { get; set; }
        public int? Birthday { get; set; }
        public int? Birthmonth { get; set; }
        public int? Birthyear { get; set; }
        public int? Icquin { get; set; }
        public short Quietflags { get; set; }
        public string Streetaddr { get; set; }
        public string Streeaddr { get; set; }
        public string City { get; set; }
        public int? Cpubrandid { get; set; }
        public int? Cpuspeed { get; set; }
        public short? Memory { get; set; }
        public string Videocard1string { get; set; }
        public short? Videocard1ram { get; set; }
        public string Videocard2string { get; set; }
        public short? Videocard2ram { get; set; }
        public int? Subscription { get; set; }
        public int? Adminrights { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Addrequest> Addrequests { get; set; }
        public virtual ICollection<Blocked> Blockeds { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }
        public virtual ICollection<Pstorage> Pstorages { get; set; }
        public virtual ICollection<Subprofile> Subprofiles { get; set; }
    }
}
