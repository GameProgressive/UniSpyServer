using GameStatus.Entity.Enumerate;
using GameStatus.Handler.SystemHandler;
using GameStatus.Network;
using Serilog.Events;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;

namespace GameStatus.Abstraction.BaseClass
{
    /// <summary>
    /// we only use selfdefine error code here
    /// so we do not need to send it to client
    /// </summary>
    internal abstract class GSCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected new GSSession _session => (GSSession)base._session;
        protected new GSRequestBase _request => (GSRequestBase)base._request;
        protected new GSResultBase _result
        {
            get { return (GSResultBase)base._result; }
            set { base._result = value; }
        }
        protected GSCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override void Handle()
        {
            RequestCheck();
            if (_result.ErrorCode != GSErrorCode.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.ToMsg(_result.ErrorCode));
                return;
            }

            DataOperation();

            if (_result.ErrorCode == GSErrorCode.Database)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.ToMsg(_result.ErrorCode));
                return;
            }

            ResponseConstruct();

            if (_result.ErrorCode != GSErrorCode.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.ToMsg(_result.ErrorCode));
                return;
            }

            Response();
        }
        protected override void RequestCheck()
        {
        }

        protected override void Response()
        {
            if (!StringExtensions.CheckResponseValidation((string)_response.SendingBuffer))
            {
                return;
            }

            _session.SendAsync((string)_response.SendingBuffer);
        }
    }
}
