using System;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Handler.CmdHandler.Error;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    public abstract class PCMResponseBase : UniSpyResponseBase
    {
        protected new PCMResultBase _result
        {
            get { return (PCMResultBase)base._result; }
            set { base._result = value; }
        }

        protected new PCMRequestBase _request
        {
            get { return (PCMRequestBase)base._request; }
        }

        public new string SendingBuffer
        {
            get { return (string)base.SendingBuffer; }
            protected set { base.SendingBuffer = value; }
        }
     
        protected PCMResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
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
