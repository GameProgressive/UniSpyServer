using GameSpyLib.Abstraction.BaseClass;
using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using Serilog.Events;
using StatsAndTracking.Entity.Enumerator;
using StatsAndTracking.Handler.SystemHandler;
using System.Collections.Generic;
//we store base class here but the namespace is not changed
namespace StatsAndTracking.Handler.CommandHandler
{
    /// <summary>
    /// we only use selfdefine error code here
    /// so we do not need to send it to client
    /// </summary>
    public abstract class GStatsCommandHandlerBase : CommandHandlerBase
    {
        protected string _sendingBuffer;
        protected GStatsErrorCode _errorCode;
        protected new GStatsSession _session;
        protected GStatsCommandHandlerBase(ISession session, Dictionary<string, string> request) : base(session)
        {
            _errorCode = GStatsErrorCode.NoError;
            _session = (GStatsSession)session.GetInstance();
        }

        public override void Handle()
        {
            base.Handle();
            CheckRequest();
            if (_errorCode != GStatsErrorCode.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.ToMsg(_errorCode));
                return;
            }

            DataOperation();

            if (_errorCode == GStatsErrorCode.Database)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.ToMsg(_errorCode));
                return;
            }

            ConstructResponse();

            if (_errorCode != GStatsErrorCode.NoError)
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
