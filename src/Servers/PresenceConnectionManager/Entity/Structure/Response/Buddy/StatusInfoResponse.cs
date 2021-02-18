using System;
using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Result.Buddy;
using PresenceConnectionManager.Structure.Data;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response.Buddy
{
    internal sealed class StatusInfoResponse : PCMResponseBase
    {
        private new StatusInfoResult _result => (StatusInfoResult)base._result;

        public StatusInfoResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        /// <summary>
        /// \bsi\\state\\profile\\bip\\bport\\hostip\\hprivip\\qport\\hport\\sessflags\\rstatus\\
        /// gameType\\gameVnt\\gameMn\\product\\qmodeflags\
        /// </summary>
        protected override void BuildNormalResponse()
        {
            SendingBuffer = @$"bsi\\state\{_result.StatusInfo.StatusState}\
                profile\{_result.ProfileID}\bip\{_result.StatusInfo.BuddyIP}
                hostIp\{_result.StatusInfo}\
                hprivIp\{_result.StatusInfo.HostPrivateIP}\qport\{_result.StatusInfo.QueryReportPort}\
                hport\{_result.StatusInfo.HostPort}\sessflags\{_result.StatusInfo.SessionFlags}\
                rstatus\{_result.StatusInfo.RichStatus}\gameType\{_result.StatusInfo.GameType}\
                gameVnt\{_result.StatusInfo.GameVariant}\gameMn\{_result.StatusInfo.GameMapName}\
                product\{_result.ProductID}\qmodeflags\{_result.StatusInfo.QuietModeFlags}final\";
        }
    }
}
