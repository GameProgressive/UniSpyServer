namespace UniSpy.Server.Core.Database.DatabaseModel
{
    /// <summary>
    /// Block list.
    /// </summary>
    public partial class Blocked
    {
        public int Blockid { get; set; }
        public int Profileid { get; set; }
        public int Namespaceid { get; set; }
        public int Targetid { get; set; }

        public virtual Profile Profile { get; set; } = null!;
    }
}
