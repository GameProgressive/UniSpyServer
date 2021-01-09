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
        protected new PSPRequestBase _request
        {
            get { return (PSPRequestBase)base._request; }
        }
        public new string SendingBuffer
        {
            get { return (string)base.SendingBuffer; }
            protected set { base.SendingBuffer = value; }
        }
        protected PSPResponseBase(PSPRequestBase request, UniSpyResultBase result) : base(request, result)
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
