namespace UniSpyServer.UniSpyLib.Database.DatabaseModel
{
    /// <summary>
    /// User subprofiles.
    /// </summary>
    public partial class Subprofile
    {
        public int SubProfileId { get; set; }
        public int ProfileId { get; set; }
        public string Uniquenick { get; set; }
        public int NamespaceId { get; set; }
        public int PartnerId { get; set; }
        public int? ProductId { get; set; }
        public string Gamename { get; set; }
        public string Cdkeyenc { get; set; }
        public short? Firewall { get; set; }
        public int? Port { get; set; }
        public string Authtoken { get; set; }

        public virtual Profile Profile { get; set; } = null!;
    }
}
