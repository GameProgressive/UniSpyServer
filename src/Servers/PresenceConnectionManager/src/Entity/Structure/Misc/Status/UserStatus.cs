using UniSpy.Server.PresenceConnectionManager.Entity.Enumerate;

namespace UniSpy.Server.PresenceConnectionManager.Entity.Structure.Misc
{
    /// <summary>
    /// This status class is for old SDK version
    /// </summary>
    public sealed class UserStatus
    {
        public GPStatusCode CurrentStatus { get; set; }
        public string StatusString { get; set; }
        public string LocationString { get; set; }
        public UserStatus()
        {
            CurrentStatus = GPStatusCode.Offline;
            StatusString = "";
            LocationString = "";
        }
    }
}
