using PresenceConnectionManager.Entity.Enumerate;

namespace PresenceConnectionManager.Entity.Structure.Misc
{
    /// <summary>
    /// This status class is for old SDK version
    /// </summary>
    internal class PCMUserStatus
    {
        public GPStatusCode CurrentStatus { get; set; }
        public string StatusString { get; set; }
        public string LocationString { get; set; }
        public PCMUserStatus()
        {
            CurrentStatus = GPStatusCode.Offline;
            StatusString = "";
            LocationString = "";
        }
    }
}
