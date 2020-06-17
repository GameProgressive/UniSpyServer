namespace GameSpyLib.Database.DatabaseModel.MySql
{
    public partial class Grouplist
    {
        public uint Groupid { get; set; }
        public uint Gameid { get; set; }
        public string Roomname { get; set; }

        public virtual Games Game { get; set; }
    }
}
