using StatsAndTracking.Entity.Enumerator;
using StatsAndTracking.Handler.SystemHandler;
using System.Collections.Generic;

namespace StatsAndTracking.Handler.CommandHandler
{
    public class GStatsHandlerBase
    {
        protected string _sendingBuffer;
        protected uint _localId;
        protected GstatsErrorCode _errorCode = GstatsErrorCode.NoError;
        protected GStatsHandlerBase(GStatsSession session, Dictionary<string, string> recv)
        {
            Handle(session, recv);
        }

        protected virtual void Handle(GStatsSession session, Dictionary<string, string> recv)
        {
            CheckRequest(session, recv);
            if (_errorCode != GstatsErrorCode.NoError)
            {
                session.ToLog(ErrorMessage.ToMsg(_errorCode));
                return;
            }
            DatabaseOperation(session, recv);
            if (_errorCode == GstatsErrorCode.Database)
            {
                session.ToLog(ErrorMessage.ToMsg(_errorCode));
                return;
            }
            ConstructResponse(session, recv);
            if (_errorCode != GstatsErrorCode.NoError)
            {
                session.ToLog(ErrorMessage.ToMsg(_errorCode));
                return;
            }
            Response(session, recv);

        }

        protected virtual void CheckRequest(GStatsSession session, Dictionary<string, string> recv)
        {
            //worms 3d use id not lid so we added conditions here
            if (!recv.ContainsKey("lid") || !recv.ContainsKey("id"))
            {
                _errorCode = GstatsErrorCode.Parse;
                return;
            }

            if (recv.ContainsKey("lid"))
                if (!uint.TryParse(recv["lid"], out _localId))
                {
                    _errorCode = GstatsErrorCode.Parse;
                    return;
                }

            if (recv.ContainsKey("id"))
                if (!uint.TryParse(recv["id"], out _localId))
                {
                    _errorCode = GstatsErrorCode.Parse;
                    return;
                }
        }
        protected virtual void DatabaseOperation(GStatsSession session, Dictionary<string, string> recv)
        { }
        protected virtual void ConstructResponse(GStatsSession session, Dictionary<string, string> recv)
        { }
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
