namespace GameSpyLib.Database.DatabaseModel.MySql
{
    public partial class Games
    {
        public int Gameid { get; set; }
        public string Gamename { get; set; }
        public string Secretkey { get; set; }
        public string Description { get; set; }
        public int Queryport { get; set; }
    }
}
