namespace GameSpyLib.Database.DatabaseModel.MySql
{
    public partial class Addrequests
    {
        public uint Id { get; set; }
        public uint Profileid { get; set; }
        public uint Namespaceid { get; set; }
        public uint Targetid { get; set; }
        public string Reason { get; set; }
        public string Syncrequested { get; set; }

        public virtual Profiles Profile { get; set; }
    }
}
