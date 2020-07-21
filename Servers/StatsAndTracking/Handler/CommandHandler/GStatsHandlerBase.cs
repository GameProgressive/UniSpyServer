using GameSpyLib.Logging;
using Serilog.Events;
using StatsAndTracking.Entity.Enumerator;
using StatsAndTracking.Handler.SystemHandler;
using System.Collections.Generic;

namespace StatsAndTracking.Handler.CommandHandler
{
    /// <summary>
    /// we only use selfdefine error code here
    /// so we do not need to send it to client
    /// </summary>
    public class CommandHandlerBase
    {
        protected string _sendingBuffer;
        protected uint _localId;
        protected GStatsErrorCode _errorCode = GStatsErrorCode.NoError;

        protected CommandHandlerBase()
        {
        }

        public virtual void Handle(GStatsSession session, Dictionary<string, string> recv)
        {
            LogWriter.LogCurrentClass(this);

            CheckRequest(session, recv);
            if (_errorCode != GStatsErrorCode.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.ToMsg(_errorCode));
                return;
            }

            DataOperation(session, recv);

            if (_errorCode == GStatsErrorCode.Database)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.ToMsg(_errorCode));
                return;
            }

            ConstructResponse(session, recv);

            if (_errorCode != GStatsErrorCode.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.ToMsg(_errorCode));
                return;
            }

            Response(session, recv);
        }

        protected virtual void CheckRequest(GStatsSession session, Dictionary<string, string> recv)
        {
            if (recv.ContainsKey("lid"))
            {
                if (!uint.TryParse(recv["lid"], out _localId))
                {
                    _errorCode = GStatsErrorCode.Parse;
                }
            }

            //worms 3d use id not lid so we added an condition here
            if (recv.ContainsKey("id"))
            {
                if (!uint.TryParse(recv["id"], out _localId))
                {
                    _errorCode = GStatsErrorCode.Parse;
                }
            }
        }

        protected virtual void DataOperation(GStatsSession session, Dictionary<string, string> recv)
        {
        }

        protected virtual void ConstructResponse(GStatsSession session, Dictionary<string, string> recv)
        {
        }

        protected virtual void Response(GStatsSession session, Dictionary<string, string> recv)
        {
            if (_sendingBuffer == null)
            {
                return;
            }

            session.SendAsync(_sendingBuffer);
        }
    }
}
