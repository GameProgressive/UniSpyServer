namespace UniSpyServer.UniSpyLib.Database.DatabaseModel
{
    /// <summary>
    /// Friend list.
    /// </summary>
    public partial class Friend
    {
        public int Friendid { get; set; }
        public int ProfileId { get; set; }
        public int Namespaceid { get; set; }
        public int Targetid { get; set; }

        public virtual Profile Profile { get; set; } = null!;
    }
}
