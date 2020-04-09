using PresenceSearchPlayer.Enumerator;
using PresenceSearchPlayer.Handler.CommandHandler.Error;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.CommandHandler
{
    public class CommandHandlerBase
    {
        protected GPErrorCode _errorCode = GPErrorCode.NoError;
        /// <summary>
        /// Be careful the return of query function should be List type,
        /// the decision formula should use _result.Count==0
        /// </summary>
        protected string _sendingBuffer;
        protected ushort _operationID;
        protected uint _namespaceid = 0;

        protected CommandHandlerBase()
        {
        }

        public virtual void Handle(GPSPSession session, Dictionary<string, string> recv)
        {
            CheckRequest(session, recv);

            if (_errorCode != GPErrorCode.NoError)
            {
                ErrorMsg.SendGPSPError(session, _errorCode, _operationID);
                return;
            }

            DataOperation(session, recv);

            if (_errorCode == GPErrorCode.DatabaseError)
            {
                ErrorMsg.SendGPSPError(session, _errorCode, _operationID);
                return;
            }

            ConstructResponse(session, recv);

            if (_errorCode == GPErrorCode.ConstructResponseError)
            {
                ErrorMsg.SendGPSPError(session, GPErrorCode.General, _operationID);
                return;
            }

            Response(session, recv);
        }

        protected virtual void CheckRequest(GPSPSession session, Dictionary<string, string> recv)
        {
            if (recv.ContainsKey("id"))
            {
                if (!ushort.TryParse(recv["id"], out _operationID))
                {
                    //default operationID
                    session.OperationID = 1;
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

        protected virtual void DataOperation(GPSPSession session, Dictionary<string, string> recv) { }

        protected virtual void ConstructResponse(GPSPSession session, Dictionary<string, string> recv) { }

        protected virtual void Response(GPSPSession session, Dictionary<string, string> recv)
        {
            if (_sendingBuffer != null)
            {
                session.SendAsync(_sendingBuffer);
            }
        }
    }
}
