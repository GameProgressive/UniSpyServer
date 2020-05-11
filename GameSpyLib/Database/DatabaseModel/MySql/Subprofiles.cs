namespace GameSpyLib.Database.DatabaseModel.MySql
{
    public partial class Subprofiles
    {
        public uint Subprofileid { get; set; }
        public uint Profileid { get; set; }
        public uint Namespaceid { get; set; }
        public uint Partnerid { get; set; }
        public uint? Productid { get; set; }
        public string Uniquenick { get; set; }
        public string Gamename { get; set; }
        public string Cdkeyenc { get; set; }
        public bool? Firewall { get; set; }
        public uint? Port { get; set; }
        public string Authtoken { get; set; }

        public virtual Partner Partner { get; set; }
        public virtual Profiles Profile { get; set; }
    }
}
