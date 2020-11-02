namespace UniSpyLib.Database.DatabaseModel.MySql
{
    public partial class Statusinfo
    {
        public uint Statusinfoid { get; set; }
        public uint Profileid { get; set; }
        public uint Namespaceid { get; set; }
        public uint Productid { get; set; }
        public string Statusstate { get; set; }
        public string Buddyip { get; set; }
        public string Hostip { get; set; }
        public string Hostprivateip { get; set; }
        public uint? Queryreport { get; set; }
        public uint? Hostport { get; set; }
        public uint? Sessionflags { get; set; }
        public string Richstatus { get; set; }
        public string Gametype { get; set; }
        public string Gamevariant { get; set; }
        public string Gamemapname { get; set; }
        public string Quietmodefalgs { get; set; }

        public virtual Profiles Profile { get; set; }
    }
}
