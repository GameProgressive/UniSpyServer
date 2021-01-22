using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Logging;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class SBResponseBase : UniSpyResponseBase
    {
        protected new SBRequestBase _request => (SBRequestBase)base._request;
        protected new SBResultBase _result => (SBResultBase)base._result;
        protected new byte[] SendingBuffer
        {
            get => (byte[])base.SendingBuffer;
            set => base.SendingBuffer = value;
        }
        protected SBResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            if (_result.ErrorCode != Entity.Enumerate.SBErrorCode.NoError)
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
            LogWriter.ToLog(Serilog.Events.LogEventLevel.Error, _result.ErrorCode.ToString());
        }
    }
}
