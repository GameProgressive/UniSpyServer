using System.Collections.Generic;

namespace GameSpyLib.Database.DatabaseModel.MySql
{
    public partial class Profiles
    {
        public Profiles()
        {
            Addrequests = new HashSet<Addrequests>();
            Blocked = new HashSet<Blocked>();
            Friends = new HashSet<Friends>();
            Pstorage = new HashSet<Pstorage>();
            Statusinfo = new HashSet<Statusinfo>();
            Subprofiles = new HashSet<Subprofiles>();
        }

        public uint Id { get; set; }
        public uint Userid { get; set; }
        public string Nick { get; set; }
        public int Serverflag { get; set; }
        public byte? Status { get; set; }
        public string Statstring { get; set; }
        public string Location { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Publicmask { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Aim { get; set; }
        public int Picture { get; set; }
        public int? Occupationid { get; set; }
        public int? Incomeid { get; set; }
        public int? Industryid { get; set; }
        public int? Marriedid { get; set; }
        public int? Childcount { get; set; }
        public int? Interests1 { get; set; }
        public int? Ownership1 { get; set; }
        public int? Connectiontype { get; set; }
        public byte? Sex { get; set; }
        public string Zipcode { get; set; }
        public string Countrycode { get; set; }
        public string Homepage { get; set; }
        public int? Birthday { get; set; }
        public int? Birthmonth { get; set; }
        public int? Birthyear { get; set; }
        public uint? Icquin { get; set; }
        public sbyte Quietflags { get; set; }
        public string Streetaddr { get; set; }
        public string Streeaddr { get; set; }
        public string City { get; set; }
        public int Cpubrandid { get; set; }
        public short Cpuspeed { get; set; }
        public sbyte Memory { get; set; }
        public string Videocard1string { get; set; }
        public sbyte Videocard1ram { get; set; }
        public string Videocard2string { get; set; }
        public sbyte Videocard2ram { get; set; }
        public int Subscription { get; set; }
        public int Adminrights { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<Addrequests> Addrequests { get; set; }
        public virtual ICollection<Blocked> Blocked { get; set; }
        public virtual ICollection<Friends> Friends { get; set; }
        public virtual ICollection<Pstorage> Pstorage { get; set; }
        public virtual ICollection<Statusinfo> Statusinfo { get; set; }
        public virtual ICollection<Subprofiles> Subprofiles { get; set; }
    }
}
