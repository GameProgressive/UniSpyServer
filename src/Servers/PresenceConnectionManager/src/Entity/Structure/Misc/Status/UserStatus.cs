using UniSpyServer.Servers.PresenceConnectionManager.Entity.Enumerate;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Misc
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
