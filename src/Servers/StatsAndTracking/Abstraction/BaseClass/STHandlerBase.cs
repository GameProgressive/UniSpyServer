using GameSpyLib.Abstraction.BaseClass;
using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using Serilog.Events;
using StatsTracking.Entity.Enumerate;
using StatsTracking.Handler.SystemHandler;
using System.Collections.Generic;
//we store base class here but the namespace is not changed
namespace StatsTracking.Abstraction.BaseClass
{
    /// <summary>
    /// we only use selfdefine error code here
    /// so we do not need to send it to client
    /// </summary>
    public abstract class STCommandHandlerBase : CommandHandlerBase
    {
        protected string _sendingBuffer;
        protected STError _errorCode;
        protected new STSession _session;
        protected STCommandHandlerBase(ISession session, Dictionary<string, string> request) : base(session)
        {
            _errorCode = STError.NoError;
            _session = (STSession)session.GetInstance();
        }

        public override void Handle()
        {
            base.Handle();
            CheckRequest();
            if (_errorCode != STError.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.ToMsg(_errorCode));
                return;
            }

            DataOperation();

            if (_errorCode == STError.Database)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.ToMsg(_errorCode));
                return;
            }

            ConstructResponse();

            if (_errorCode != STError.NoError)
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
