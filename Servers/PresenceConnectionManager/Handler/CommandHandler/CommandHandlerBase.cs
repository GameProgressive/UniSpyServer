using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.Error;
using System.Collections.Generic;


namespace PresenceConnectionManager.Handler
{
    public class CommandHandlerBase
    {
        protected GPErrorCode _errorCode = GPErrorCode.NoError;
        protected string _sendingBuffer;
        protected ushort _operationID;
        protected uint _namespaceid = 0;

        protected CommandHandlerBase(GPCMSession session, Dictionary<string, string> recv)
        {
            Handle(session, recv);
        }

        public virtual void Handle(GPCMSession session, Dictionary<string, string> recv)
        {
            CheckRequest(session, recv);
            if (_errorCode != GPErrorCode.NoError)
            {
                //TODO
                ErrorMsg.SendGPCMError(session, _errorCode, _operationID);
                return;
            }

            DataOperation(session, recv);
            if (_errorCode == GPErrorCode.DatabaseError)
            {
                //TODO
                ErrorMsg.SendGPCMError(session, _errorCode, _operationID);
                return;
            }

            ConstructResponse(session, recv);
            if (_errorCode == GPErrorCode.ConstructResponseError)
            {
                ErrorMsg.SendGPCMError(session, _errorCode, _operationID);
                return;
            }

            Response(session, recv);
        }

        protected virtual void CheckRequest(GPCMSession session, Dictionary<string, string> recv)
        {
            if (recv.ContainsKey("id"))
            {
                if (!ushort.TryParse(recv["id"], out _operationID))
                {
                    _errorCode = GPErrorCode.Parse;
                }
            }
            if (recv.ContainsKey("namespaceid"))
            {
                if (!uint.TryParse(recv["namespaceid"], out _namespaceid))
                {
                    _errorCode = GPErrorCode.Parse;
                }
            }
        }

        protected virtual void DataOperation(GPCMSession session, Dictionary<string, string> recv) { }

        protected virtual void ConstructResponse(GPCMSession session, Dictionary<string, string> recv) { }

        protected virtual void Response(GPCMSession session, Dictionary<string, string> recv)
        {
            if (_sendingBuffer == null)
            {
                return;
            }
            session.SendAsync(_sendingBuffer);
        }
    }
}
