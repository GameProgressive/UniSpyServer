using System;
using GameSpyLib.Logging;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Handler.SystemHandler.Error;
using Serilog.Events;
namespace ServerBrowser.Handler.CommandHandler
{
    public abstract class CommandHandlerBase
    {
        protected SBErrorCode _errorCode = SBErrorCode.NoError;
        protected byte[] _sendingBuffer;

        public CommandHandlerBase(SBSession session, byte[] recv)
        {
            Handle(session, recv);
        }

        public virtual void Handle(SBSession session, byte[] recv)
        {
            CheckRequest(session, recv);

            if (_errorCode != SBErrorCode.NoError)
            {
                session.ToLog(LogEventLevel.Error, ErrorMessage.GetErrorMsg(_errorCode));
                return;
            }

            DataOperation(session, recv);

            if (_errorCode != SBErrorCode.NoError)
            {
                session.ToLog(LogEventLevel.Error, ErrorMessage.GetErrorMsg(_errorCode));
                return;
            }

            ConstructResponse(session, recv);

            if (_errorCode != SBErrorCode.NoError)
            {
                session.ToLog(LogEventLevel.Error, ErrorMessage.GetErrorMsg(_errorCode));
                return;
            }

            Response(session, recv);
        }

        public virtual void CheckRequest(SBSession session, byte[] recv)
        {
        }

        public virtual void DataOperation(SBSession session, byte[] recv)
        {
        }

        public virtual void ConstructResponse(SBSession session, byte[] recv)
        {
        }

        public virtual void Response(SBSession session, byte[] recv)
        {
            if (_sendingBuffer == null || _sendingBuffer.Length < 4)
            {
                return;
            }

            session.SendAsync(_sendingBuffer);
        }
    }
}
