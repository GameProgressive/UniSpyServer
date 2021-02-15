using System;
namespace PresenceConnectionManager.Entity.Structure.Misc
{
    public class PCMStatusInfo
    {
        public string StatusState { get; set; }
        public string BuddyIP { get; set; }
        public string HostIP { get; set; }
        public string HostPrivateIP { get; set; }
        public uint? QueryReportPort { get; set; }
        public uint? HostPort { get; set; }
        public uint? SessionFlags { get; set; }
        public string RichStatus { get; set; }
        public string GameType { get; set; }
        public string GameVariant { get; set; }
        public string GameMapName { get; set; }
        public string QuietModeFlags { get; set; }
        public PCMStatusInfo()
        {
        }
    }
}
