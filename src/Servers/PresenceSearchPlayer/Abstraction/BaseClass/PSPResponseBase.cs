using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Handler.CmdHandler.Error;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Abstraction.BaseClass
{
    public abstract class PSPResponseBase : UniSpyResponseBase
    {
        protected new PSPResultBase _result
        {
            get { return (PSPResultBase)base._result; }
        }

        public PSPResponseBase(PSPResultBase result) : base(result)
        {
        }

        public override void Build()
        {
            if (_result.ErrorCode != GPErrorCode.NoError)
            {
                BuildErrorResponse();
            }
            else
            {
                BuildNormalResponse();
            }
        }

        protected override void BuildErrorResponse()
        {
            SendingBuffer = ErrorMsg.BuildGPErrorMsg(_result.ErrorCode);
        }
    }
}
