using System;
using GameSpyLib.Common;
using System.Collections.Generic;
using PresenceConnectionManager.Enumerator;

namespace PresenceConnectionManager.Handler
{
    public class GPCMHandlerBase : HandlerBase<GPCMSession, Dictionary<string, string>>
    {
        protected Dictionary<string, string> _recv;
        protected GPErrorCode _errorCode = GPErrorCode.NoError;
        protected Dictionary<string, object> _result;
        protected string _sendingBuffer;

        protected GPCMHandlerBase(Dictionary<string, string> recv)
        {
            _recv = recv;
        }
        public override void Handle(GPCMSession session)
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

        public virtual void CheckRequest(GPCMSession session) { }

        public virtual void DataBaseOperation(GPCMSession session) { }

        public virtual void CheckDatabaseResult(GPCMSession session) { }

        public virtual void Response(GPCMSession session)
        {
            if (_sendingBuffer != null)
            {
                session.SendAsync(_sendingBuffer);
            }
        }
    }
}
