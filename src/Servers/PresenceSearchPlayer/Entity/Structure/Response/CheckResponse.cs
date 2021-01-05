using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Structure.Result;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    public class CheckResponse : PSPResponseBase
    {
        protected new CheckResult _result
        {
            get { return (CheckResult)base._result; }
        }

        public CheckResponse(CheckResult result) : base(result)
        {
        }

        protected override void BuildErrorResponse()
        {
            if (_result.ErrorCode == GPErrorCode.Check
              || _result.ErrorCode > GPErrorCode.CheckBadPassword)
            {
                SendingBuffer = @$"\cur\{ _result.ErrorCode}\final\";
            }
            else
            {
                base.BuildErrorResponse();
            }
        }

        protected override void BuildNormalResponse()
        {
            SendingBuffer = @$"\cur\0\pid\{_result.ProfileID}\final\";
        }
    }
}
