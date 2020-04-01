namespace GameSpyLib.Database.DatabaseModel.MySql
{
    public partial class Blocked
    {
        public uint Id { get; set; }
        public uint Profileid { get; set; }
        public uint Targetid { get; set; }
        public uint Namespaceid { get; set; }
    }
}
