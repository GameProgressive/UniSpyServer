using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response
{
    public sealed class StatusInfoResponse : ResponseBase
    {
        private new StatusInfoResult _result => (StatusInfoResult)base._result;

        public StatusInfoResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        /// <summary>
        /// \bsi\\state\\profile\\bip\\bport\\hostip\\hprivip\\qport\\hport\\sessflags\\rstatus\\
        /// gameType\\gameVnt\\gameMn\\product\\qmodeflags\
        /// </summary>
        public override void Build()
        {
            SendingBuffer = $@"bsi\\state\{_result.StatusInfo.StatusState}\
                profile\{_result.ProfileId}\bip\{_result.StatusInfo.BuddyIP}
                hostIp\{_result.StatusInfo}\
                hprivIp\{_result.StatusInfo.HostPrivateIP}\qport\{_result.StatusInfo.QueryReportPort}\
                hport\{_result.StatusInfo.HostPort}\sessflags\{_result.StatusInfo.SessionFlags}\
                rstatus\{_result.StatusInfo.RichStatus}\gameType\{_result.StatusInfo.GameType}\
                gameVnt\{_result.StatusInfo.GameVariant}\gameMn\{_result.StatusInfo.GameMapName}\
                product\{_result.ProductID}\qmodeflags\{_result.StatusInfo.QuietModeFlags}final\";
        }
    }
}
