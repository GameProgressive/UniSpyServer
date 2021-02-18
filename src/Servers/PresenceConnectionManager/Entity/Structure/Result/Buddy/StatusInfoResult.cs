using System;
using System.Net;
using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Misc;
using UniSpyLib.Extensions;

namespace PresenceConnectionManager.Entity.Structure.Result.Buddy
{
    internal sealed class StatusInfoResult : PCMResultBase
    {
        public uint ProfileID { get; set; }
        public uint ProductID { get; set; }
        public PCMUserStatusInfo StatusInfo { get; set; }
        public StatusInfoResult()
        {
        }
    }
}
