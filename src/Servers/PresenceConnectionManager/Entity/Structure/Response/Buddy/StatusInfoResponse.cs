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
            SendingBuffer = @$"bsi\\state\{_result.StatusState}\
                profile\{_result.ProfileID}\bip\{_result.HtonBuddyIP}
                hostIp\{_result.HtonHostIP}\
                hprivIp\{_result.HostPrivateIP}\qport\{_result.QueryReportPort}\
                hport\{_result.HostPort}\sessflags\{_result.SessionFlags}\
                rstatus\{_result.RichStatus}\gameType\{_result.GameType}\
                gameVnt\{_result.GameVariant}\gameMn\{_result.GameMapName}\
                product\{_result.ProductID}\qmodeflags\{_result.QuietModeFlags}final\";
        }
    }
}
