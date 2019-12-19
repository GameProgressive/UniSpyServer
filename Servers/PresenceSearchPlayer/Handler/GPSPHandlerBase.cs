using System;
using System.Collections.Generic;
using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Handler
{
    public class GPSPHandlerBase:HandlerBase<GPSPSession,Dictionary<string,string>>
    {
        protected Dictionary<string, string> _recv;
        protected GPErrorCode _errorCode = GPErrorCode.NoError;
        protected Dictionary<string, object> _result;
        protected string _sendingBuffer;

        protected GPSPHandlerBase(Dictionary<string, string> recv)
        {
            _recv = recv;
        }
        public override void Handle(GPSPSession session)
        {
            CheckRequest(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                //TODO
            }

            DataBaseOperation(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                //TODO
            }

            CheckDatabaseResult(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                //TODO
            }

            Response(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                //TODO
            }
        }

        public virtual void CheckRequest(GPSPSession session) { }

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
