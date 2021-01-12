using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;
using Serilog.Events;
using GameStatus.Entity.Enumerate;
using GameStatus.Handler.SystemHandler;
using System.Collections.Generic;
using GameStatus.Network;
//we store base class here but the namespace is not changed
namespace GameStatus.Abstraction.BaseClass
{
    /// <summary>
    /// we only use selfdefine error code here
    /// so we do not need to send it to client
    /// </summary>
    internal abstract class GSCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected string _sendingBuffer;
        protected GSErrorCode _errorCode;
        protected new GSSession _session
        {
            get { return (GSSession)base._session; }
        }
        protected GSCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session,request)
        {
            _errorCode = GSErrorCode.NoError;
        }

        public override void Handle()
        {
            RequestCheck();
            if (_errorCode != GSErrorCode.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.ToMsg(_errorCode));
                return;
            }

            DataOperation();

            if (_errorCode == GSErrorCode.Database)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.ToMsg(_errorCode));
                return;
            }

            ResponseConstruct();

            if (_errorCode != GSErrorCode.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.ToMsg(_errorCode));
                return;
            }

            Response();
        }
        protected override void RequestCheck()
        {
        }
        protected override void Response()
        {
            if (!StringExtensions.CheckResponseValidation(_sendingBuffer))
            {
                return;
            }

            _session.SendAsync(_sendingBuffer);
        }
    }
}
