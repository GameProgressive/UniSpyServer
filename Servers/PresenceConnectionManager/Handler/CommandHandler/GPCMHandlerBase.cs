using GameSpyLib.Common;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.Error;
using System.Collections.Generic;


namespace PresenceConnectionManager.Handler
{
    public class GPCMHandlerBase : HandlerBase<GPCMSession, Dictionary<string, string>>
    {
        protected Dictionary<string, string> _recv;
        protected GPErrorCode _errorCode = GPErrorCode.NoError;
        protected List<Dictionary<string, object>> _result;
        protected string _sendingBuffer;
        protected DisconnectReason _discReason;
        protected ushort _operationID;

        protected GPCMHandlerBase(Dictionary<string, string> recv)
        {
            _recv = recv;
        }
        protected GPCMHandlerBase() { }

        public override void Handle(GPCMSession session)
        {
            CheckRequest(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                //TODO
                ErrorMsg.SendGPCMError(session, _errorCode, _operationID);
                return;
            }

            DataBaseOperation(session);
            if (_errorCode == GPErrorCode.DatabaseError)
            {
                //TODO
                ErrorMsg.SendGPCMError(session, _errorCode, _operationID);
                return;
            }

            ConstructResponse(session);
            if (_errorCode == GPErrorCode.ConstructResponseError)
            {
                ErrorMsg.SendGPCMError(session, _errorCode, _operationID);
                return;
            }

            Response(session);
        }
        protected virtual void CheckRequest(GPCMSession session)
        {
            if (_recv.ContainsKey("id"))
                if (!ushort.TryParse(_recv["id"], out _operationID))
                {
                    _errorCode = GPErrorCode.Parse;
                }
        }

        protected virtual void DataBaseOperation(GPCMSession session) { }

        protected virtual void ConstructResponse(GPCMSession session) { }

        protected virtual void Response(GPCMSession session)
        {
            if (_sendingBuffer == null)
            {
                return;
            }
            session.SendAsync(_sendingBuffer);
        }
    }
}
