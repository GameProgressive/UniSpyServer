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
                ErrorMsg.SendGPSPError(session, _errorCode, _operationID);
                return;
            }

            DataBaseOperation(session);
            if (_errorCode == GPErrorCode.DatabaseError)
            {
                ErrorMsg.SendGPSPError(session, _errorCode, _operationID);
                return;
            }

            ConstructResponse(session);
            if (_errorCode == GPErrorCode.ConstructResponseError)
            {
                ErrorMsg.SendGPSPError(session, _errorCode, _operationID);
                return;
            }

            Response(session);
        }

        protected virtual void CheckRequest(GPSPSession session) 
        {
            if (_recv.ContainsKey("id"))
            {
                if (!UInt16.TryParse(_recv["id"], out _operationID))
                {
                    //default operationID
                    _operationID = 1;
                    session.OperationID = 1;
                }
            }
            
        }

        protected virtual void DataBaseOperation(GPSPSession session) { }

        protected virtual void CheckDatabaseResult(GPSPSession session) { }

        protected virtual void ConstructResponse(GPSPSession session) { }

        protected virtual void Response(GPSPSession session)
        {
            if (_sendingBuffer != null)
            {
                session.SendAsync(_sendingBuffer);
            }
        }
    }
}
