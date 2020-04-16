using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using PresenceSearchPlayer.Enumerator;
using PresenceSearchPlayer.Handler.CommandHandler.Error;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.CommandHandler
{
    public class PSPCommandHandlerBase : CommandHandlerBase
    {
        protected GPErrorCode _errorCode;
        /// <summary>
        /// Be careful the return of query function should be List type,
        /// the decision formula should use _result.Count==0
        /// </summary>
        protected string _sendingBuffer;
        protected ushort _operationID;
        protected uint _namespaceid ;
        protected Dictionary<string, string> _recv;

        public PSPCommandHandlerBase(IClient client, Dictionary<string, string> recv) : base(client)
        {
            _recv = recv;
            _errorCode = GPErrorCode.NoError;
            _namespaceid = 0;
            _operationID = 1;
        }

        public override void Handle()
        {
            LogWriter.LogCurrentClass(this);

            CheckRequest();

            if (_errorCode != GPErrorCode.NoError)
            {
                ErrorMsg.SendGPSPError(_client, _errorCode, _operationID);
                return;
            }

            DataOperation();

            if (_errorCode == GPErrorCode.DatabaseError)
            {
                ErrorMsg.SendGPSPError(_client, _errorCode, _operationID);
                return;
            }

            ConstructResponse();

            if (_errorCode == GPErrorCode.ConstructResponseError)
            {
                ErrorMsg.SendGPSPError(_client, GPErrorCode.General, _operationID);
                return;
            }

            Response();
        }

        protected virtual void CheckRequest()
        {
            if (_recv.ContainsKey("id"))
            {
                if (!ushort.TryParse(_recv["id"], out _operationID))
                {
                    _errorCode = GPErrorCode.Parse;
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

        protected virtual void DataOperation() { }

        protected virtual void ConstructResponse() { }

        protected virtual void Response()
        {
            if (_sendingBuffer != null)
            {
                _client.SendAsync(_sendingBuffer);
            }
        }
    }
}
