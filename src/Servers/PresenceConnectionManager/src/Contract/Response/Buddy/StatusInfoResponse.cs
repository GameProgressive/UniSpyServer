using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Contract.Result;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Response
{
    public sealed class StatusInfoResponse : ResponseBase
    {
        private new StatusInfoResult _result => (StatusInfoResult)base._result;

        public StatusInfoResponse(RequestBase request, ResultBase result) : base(request, result)
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
                product\{_result.ProductId}\qmodeflags\{_result.StatusInfo.QuietModeFlags}final\";
        }
    }
}
