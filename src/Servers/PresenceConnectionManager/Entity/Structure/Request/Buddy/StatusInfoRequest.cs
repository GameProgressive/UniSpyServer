using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Buddy
{
    public class StatusInfoRequest : PCMRequestBase
    {
        public string StatusState { get; protected set; }
        public string BuddyIP { get; protected set; }
        public string HostIP { get; protected set; }
        public string HostPrivateIP { get; protected set; }
        public uint? QueryReportPort { get; protected set; }
        public uint? HostPort { get; protected set; }
        public uint? SessionFlags { get; protected set; }
        public string RichStatus { get; protected set; }
        public string GameType { get; protected set; }
        public string GameVariant { get; protected set; }
        public string GameMapName { get; protected set; }
        public string QuietModeFlags { get; protected set; }

        public StatusInfoRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPError Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }
            if (!_recv.ContainsKey("state")
                || !_recv.ContainsKey("hostIp")
                || !_recv.ContainsKey("hprivIp")
                || !_recv.ContainsKey("qport")
                || !_recv.ContainsKey("hport")
                || !_recv.ContainsKey("sessflags")
                || !_recv.ContainsKey("rechStatus")
                || !_recv.ContainsKey("gameType")
                || !_recv.ContainsKey("gameVariant")
                || !_recv.ContainsKey("gameMapName"))
            {
                return GPError.Parse;
            }

            StatusState = _recv["state"];
            HostIP = _recv["hostIp"];
            HostPrivateIP = _recv["hprivIp"];

            uint qport;
            if (!uint.TryParse(_recv["qport"], out qport))
            {
                return GPError.Parse;
            }
            QueryReportPort = qport;
            uint hport;
            if (uint.TryParse(_recv["hport"], out hport))
            {
                return GPError.Parse;
            }
            HostPort = hport;

            uint sessflags;
            if (!uint.TryParse(_recv["sessflags"], out sessflags))
            {
                return GPError.Parse;
            }
            SessionFlags = sessflags;

            RichStatus = _recv["rechStatus"];
            GameType = _recv["gameType"];
            GameVariant = _recv["gameVariant"];
            GameMapName = _recv["gameMapName"];

            return GPError.NoError;
        }
    }
}
