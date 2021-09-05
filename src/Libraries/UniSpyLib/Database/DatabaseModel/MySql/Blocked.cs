namespace UniSpyLib.Database.DatabaseModel.MySql
{
    public partial class Blocked
    {
        public uint Blockid { get; set; }
        public uint Profileid { get; set; }
        public uint Namespaceid { get; set; }
        public uint Targetid { get; set; }

        public virtual Profiles Profile { get; set; }
    }
}
