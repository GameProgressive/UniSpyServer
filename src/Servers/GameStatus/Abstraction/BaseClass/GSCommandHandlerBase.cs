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
    public abstract class GSCommandHandlerBase : CommandHandlerBase
    {
        protected string _sendingBuffer;
        protected GSError _errorCode;
        protected new GSSession _session;
        protected GSCommandHandlerBase(ISession session, IRequest request) : base(session)
        {
            _errorCode = GSError.NoError;
            _session = (GSSession)session;
        }

        public override void Handle()
        {
            base.Handle();
            CheckRequest();
            if (_errorCode != GSError.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.ToMsg(_errorCode));
                return;
            }

            DataOperation();

            if (_errorCode == GSError.Database)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.ToMsg(_errorCode));
                return;
            }

            ConstructResponse();

            if (_errorCode != GSError.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.ToMsg(_errorCode));
                return;
            }

            Response();
        }

        protected virtual void CheckRequest() { }

        protected virtual void DataOperation() { }

        protected virtual void ConstructResponse() { }

        protected virtual void Response()
        {
            if (!StringExtensions.CheckResponseValidation(_sendingBuffer))
            {
                return;
            }

            _session.SendAsync(_sendingBuffer);
        }
    }
}
