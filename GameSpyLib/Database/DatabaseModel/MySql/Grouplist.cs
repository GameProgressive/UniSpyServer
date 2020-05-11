namespace GameSpyLib.Database.DatabaseModel.MySql
{
    public partial class Grouplist
    {
        public int Id { get; set; }
        public int Gameid { get; set; }
        public string Roomname { get; set; }

        public virtual Games Game { get; set; }
    }
}
