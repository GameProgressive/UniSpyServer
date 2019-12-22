using System;
using System.Collections.Generic;
using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using PresenceSearchPlayer.Handler.Error;

namespace PresenceSearchPlayer.Handler
{
    public class GPSPHandlerBase:HandlerBase<GPSPSession,Dictionary<string,string>>
    {
        protected Dictionary<string, string> _recv;
        protected GPErrorCode _errorCode = GPErrorCode.NoError;
        protected Dictionary<string, object> _result;
        protected string _sendingBuffer;
        protected ushort _operationID;

        protected GPSPHandlerBase(Dictionary<string, string> recv)
        {
            _recv = recv;            
        }
        public override void Handle(GPSPSession session)
        {
            CheckRequest(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                ErrorSender.SendGPSPError(session, _errorCode, _operationID);
                return;
            }

            DataBaseOperation(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                ErrorSender.SendGPSPError(session, _errorCode, _operationID);
                return;
            }

            CheckDatabaseResult(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                ErrorSender.SendGPSPError(session, _errorCode, _operationID);
                return;
            }

            Response(session);
        }

        public virtual void CheckRequest(GPSPSession session) 
        {
            if (!UInt16.TryParse(_recv["id"], out _operationID))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        public virtual void DataBaseOperation(GPSPSession session) { }

        public virtual void CheckDatabaseResult(GPSPSession session) { }

        public virtual void Response(GPSPSession session)
        {
            if (_sendingBuffer != null)
            {
                session.SendAsync(_sendingBuffer);
            }
        }
    }
}
