namespace GameSpyLib.Database.DatabaseModel.MySql
{
    public partial class Grouplist
    {
        public int Groupid { get; set; }
        public int Gameid { get; set; }
        public int Maxwaiting { get; set; }
        public string Name { get; set; }
        public int Numplayers { get; set; }
        public int Numservers { get; set; }
        public int Numwaiting { get; set; }
        public string Other { get; set; }
        public int Password { get; set; }
        public int Updatetime { get; set; }
    }
}
