using System;
using GameSpyLib.Common;
using System.Collections.Generic;
using PresenceConnectionManager.Enumerator;

namespace PresenceConnectionManager.Handler
{
    public class GPCMHandlerBase : HandlerBase<GPCMSession>
    {
        protected Dictionary<string, string> _recv;
        protected GPErrorCode _errorCode;
        protected Dictionary<string, object> _result;
        protected string _sendingBuffer;

        protected GPCMHandlerBase(Dictionary<string, string> recv)
        {
            _recv = recv;
            _errorCode = GPErrorCode.NoError;
            _sendingBuffer = null;
        }

        public override void Handle(GPCMSession source)
        {
            throw new NotImplementedException();
        }

        protected override void CheckRequest()
        {
            throw new NotImplementedException();
        }

        protected virtual void DatabaseOperation() { }

        protected virtual void ProcessDatabaseResult() { }

        protected virtual void ConstructResponse() { }


    }
}
