using StatsAndTracking.Entity.Enumerator;
using StatsAndTracking.Handler.SystemHandler;
using System.Collections.Generic;

namespace StatsAndTracking.Handler.CommandHandler
{
    public class CommandHandlerBase
    {
        protected string _sendingBuffer;
        protected uint _localId;
        protected GstatsErrorCode _errorCode = GstatsErrorCode.NoError;

        protected CommandHandlerBase(GStatsSession session, Dictionary<string, string> recv)
        {
            Handle(session, recv);
        }

        protected virtual void Handle(GStatsSession session, Dictionary<string, string> recv)
        {
            CheckRequest(session, recv);

            if (_errorCode != GstatsErrorCode.NoError)
            {
                session.ToLog(Serilog.Events.LogEventLevel.Error, ErrorMessage.ToMsg(_errorCode));
                return;
            }

            DataOperation(session, recv);

            if (_errorCode == GstatsErrorCode.Database)
            {
                session.ToLog(Serilog.Events.LogEventLevel.Error, ErrorMessage.ToMsg(_errorCode));
                return;
            }

            ConstructResponse(session, recv);

            if (_errorCode != GstatsErrorCode.NoError)
            {
                session.ToLog(Serilog.Events.LogEventLevel.Error, ErrorMessage.ToMsg(_errorCode));
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
                    _errorCode = GstatsErrorCode.Parse;
                }
            }

            //worms 3d use id not lid so we added an condition here
            if (recv.ContainsKey("id"))
            {
                if (!uint.TryParse(recv["id"], out _localId))
                {
                    _errorCode = GstatsErrorCode.Parse;
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
