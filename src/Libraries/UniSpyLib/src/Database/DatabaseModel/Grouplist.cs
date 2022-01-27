namespace UniSpyServer.UniSpyLib.Database.DatabaseModel
{
    /// <summary>
    /// Old games use grouplist to create their game rooms.
    /// </summary>
    public partial class Grouplist
    {
        public int Groupid { get; set; }
        public int Gameid { get; set; }
        public string Roomname { get; set; } = null!;

        public virtual Game Game { get; set; } = null!;
    }
}
