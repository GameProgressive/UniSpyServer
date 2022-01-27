namespace UniSpyServer.UniSpyLib.Database.DatabaseModel
{
    /// <summary>
    /// User subprofiles.
    /// </summary>
    public partial class Subprofile
    {
        public int Subprofileid { get; set; }
        public int ProfileId { get; set; }
        public string Uniquenick { get; set; }
        public int Namespaceid { get; set; }
        public int Partnerid { get; set; }
        public int? Productid { get; set; }
        public string Gamename { get; set; }
        public string Cdkeyenc { get; set; }
        public short? Firewall { get; set; }
        public int? Port { get; set; }
        public string Authtoken { get; set; }

        public virtual Profile Profile { get; set; } = null!;
    }
}
