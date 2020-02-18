using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using PresenceSearchPlayer.Handler.CommandHandler.Error;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.CommandHandler
{
    public class GPSPHandlerBase : HandlerBase<GPSPSession, Dictionary<string, string>>
    {
        protected Dictionary<string, string> _recv;
        protected GPErrorCode _errorCode = GPErrorCode.NoError;
        /// <summary>
        /// Be careful the return of query function should be List type,
        /// the decision formula should use _result.Count==0
        /// </summary>
        protected string _sendingBuffer;
        protected ushort _operationID;
        protected uint _namespaceid = 0;

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
                ErrorMsg.SendGPSPError(session, GPErrorCode.General, _operationID);
                return;
            }

            Response(session);
        }

        protected virtual void CheckRequest(GPSPSession session)
        {
            if (_recv.ContainsKey("id"))
            {
                if (!ushort.TryParse(_recv["id"], out _operationID))
                {
                    //default operationID
                    session.OperationID = 1;
                }
            }
            if (_recv.ContainsKey("namespaceid"))
            {
                if (!uint.TryParse(_recv["namespaceid"], out _namespaceid))
                {
                    _errorCode = GPErrorCode.Parse;
                }
            }
        }

        protected virtual void DataBaseOperation(GPSPSession session) { }

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
