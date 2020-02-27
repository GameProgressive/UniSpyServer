using System;
using ServerBrowser.Entity.Enumerator;

namespace ServerBrowser.Handler.CommandHandler
{
    public class SBHandlerBase
    {
        SBErrorCode _errorCode = SBErrorCode.NoError;
        byte[] _sendingBuffer;

        public SBHandlerBase(SBSession session, byte[] recv)
        {
            Handle(session, recv);
        }
        public virtual void Handle(SBSession session, byte[] recv)
        {
            CheckRequest(session, recv);
            if (_errorCode != SBErrorCode.NoError)
            {
                return;
            }
            DataOperation(session, recv);
            if (_errorCode != SBErrorCode.NoError)
            {
                return;
            }
            ConstructResponse(session, recv);
            if (_errorCode != SBErrorCode.NoError)
            {
                return;
            }
            Response(session, recv);
        }
        public virtual void CheckRequest(SBSession session, byte[] recv)
        { }
        public virtual void DataOperation(SBSession session, byte[] recv)
        { }
        public virtual void ConstructResponse(SBSession session, byte[] recv)
        { }
        public virtual void Response(SBSession session, byte[] recv)
        {
            if (_sendingBuffer == null || _sendingBuffer.Length < 4)
            {
                return;
            }
            session.SendAsync(recv);
        }
    }
}
