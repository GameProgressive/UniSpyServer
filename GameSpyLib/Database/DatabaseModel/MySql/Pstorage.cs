namespace GameSpyLib.Database.DatabaseModel.MySql
{
    public partial class Pstorage
    {
        public uint Id { get; set; }
        public uint Profileid { get; set; }
        public uint Ptype { get; set; }
        public uint Dindex { get; set; }
        public string Data { get; set; }

        public virtual Profiles Profile { get; set; }
    }
}
