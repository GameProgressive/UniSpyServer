namespace UniSpy.Server.Core.Database.DatabaseModel
{
    /// <summary>
    /// Partner information, these information are used for authentication and login.
    /// </summary>
    public partial class Partner
    {
        public int Partnerid { get; set; }
        public string Partnername { get; set; } = null!;
    }
}
