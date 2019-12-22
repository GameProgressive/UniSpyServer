using System;
using GameSpyLib.Common;
using System.Collections.Generic;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.Error;


namespace PresenceConnectionManager.Handler
{
    public class GPCMHandlerBase : HandlerBase<GPCMSession, Dictionary<string, string>>
    {
        protected Dictionary<string, string> _recv;
        protected GPErrorCode _errorCode = GPErrorCode.NoError;
        protected Dictionary<string, object> _result;
        protected string _sendingBuffer;
        protected DisconnectReason _discReason;
        protected ushort _operationID;

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
                ErrorSender.SendGPCMError(session, _errorCode, Convert.ToUInt16(_recv["id"]));
                session.DisconnectByReason(_discReason);
                return;
            }

            DataBaseOperation(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                //TODO
                ErrorSender.SendGPCMError(session, _errorCode, Convert.ToUInt16(_recv["id"]));
                session.DisconnectByReason(_discReason);
                return;
            }

            CheckDatabaseResult(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                //TODO
                ErrorSender.SendGPCMError(session, _errorCode, Convert.ToUInt16(_recv["id"]));
                session.DisconnectByReason(_discReason);
                return;
            }

            Response(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                //TODO
                ErrorSender.SendGPCMError(session, _errorCode, Convert.ToUInt16(_recv["id"]));
                session.DisconnectByReason(_discReason);
                return;
            }
        }

        public virtual void CheckRequest(GPCMSession session)
        {
            if (!UInt16.TryParse(_recv["id"], out _operationID))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        public virtual void DataBaseOperation(GPCMSession session) { }

        public virtual void CheckDatabaseResult(GPCMSession session) { }

        public virtual void Response(GPCMSession session)
        {
            if (_sendingBuffer != null)
            {
                session.SendAsync(_sendingBuffer);
            }
            else
            {
                session.Disconnect();
            }
        }
    }
}
