namespace UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql
{
    public partial class Grouplist
    {
        public uint GroupID { get; set; }
        public uint Gameid { get; set; }
        public string Roomname { get; set; }
        public virtual Games Game { get; set; }
    }
}
