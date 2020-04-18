namespace GameSpyLib.Database.DatabaseModel.MySql
{
    public partial class Friends
    {
        public uint Id { get; set; }
        public uint Profileid { get; set; }
        public uint Targetid { get; set; }
        public uint Namespaceid { get; set; }
    }
}
