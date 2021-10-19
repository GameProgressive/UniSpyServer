namespace PresenceConnectionManager.Entity.Structure.Misc
{
    /// <summary>
    /// This status info class is for new SDK version
    /// </summary>
    public sealed class PCMUserStatusInfo
    {
        public string StatusState { get; set; }
        public string BuddyIP { get; set; }
        public string HostIP { get; set; }
        public string HostPrivateIP { get; set; }
        public int? QueryReportPort { get; set; }
        public int? HostPort { get; set; }
        public uint? SessionFlags { get; set; }
        public string RichStatus { get; set; }
        public string GameType { get; set; }
        public string GameVariant { get; set; }
        public string GameMapName { get; set; }
        public string QuietModeFlags { get; set; }
        public PCMUserStatusInfo()
        {
        }
    }
}
