using Serilog.Events;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Handler.SystemHandler.Error;
using ServerBrowser.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class SBCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected new SBResultBase _result
        {
            get => (SBResultBase)base._result;
            set => base._result = value;
        }
        protected new SBSession _session => (SBSession)base._session;

        public SBCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override void Handle()
        {
            RequestCheck();

            if (_result.ErrorCode != SBErrorCode.NoError)
            {
                ResponseConstruct();
                Response();
                return;
            }

            DataOperation();

            if (_result.ErrorCode != SBErrorCode.NoError)
            {
                ResponseConstruct();
                Response();
                return;
            }

            ResponseConstruct();
            Response();
        }

        protected override void Response()
        {
            if (_response == null)
            {
                return;
            }
            _response.Build();
            if (!StringExtensions.CheckResponseValidation((byte[])_response.SendingBuffer))
            {
                return;
            }
            _session.SendAsync((byte[])_response.SendingBuffer);
        }
    }
}
