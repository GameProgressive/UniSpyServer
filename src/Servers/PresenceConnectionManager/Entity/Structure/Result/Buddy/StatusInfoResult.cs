using System;
using System.Net;
using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Misc;
using UniSpyLib.Extensions;

namespace PresenceConnectionManager.Entity.Structure.Result.Buddy
{
    internal sealed class StatusInfoResult : PCMResultBase
    {
        public string StatusState { get; set; }
        public string BuddyIP { get; set; }
        public uint HtonBuddyIP => BitConverter.ToUInt16(HtonsExtensions.IPStringToHtonBytes(BuddyIP));
        public string HostIP { get; set; }
        public uint HtonHostIP=> BitConverter.ToUInt16(HtonsExtensions.IPStringToHtonBytes(HostIP));
        public string HostPrivateIP { get; set; }
        public uint? QueryReportPort { get; set; }
        public uint? HostPort { get; set; }
        public uint? SessionFlags { get; set; }
        public string RichStatus { get; set; }
        public string GameType { get; set; }
        public string GameVariant { get; set; }
        public string GameMapName { get; set; }
        public string QuietModeFlags { get; set; }
        public uint ProfileID { get; set; }
        public object ProductID { get; internal set; }
        public PCMUserStatusInfo StatusInfo { get; set; }
        public StatusInfoResult()
        {
        }
    }
}
