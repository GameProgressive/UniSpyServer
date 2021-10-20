namespace UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql
{
    public partial class Friends
    {
        public uint Friendid { get; set; }
        public uint Profileid { get; set; }
        public uint Namespaceid { get; set; }
        public uint Targetid { get; set; }

        public virtual Profiles Profile { get; set; }
    }
}
