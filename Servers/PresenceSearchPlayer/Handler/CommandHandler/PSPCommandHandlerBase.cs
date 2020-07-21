using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using PresenceSearchPlayer.Entity.Structure.Model;
using PresenceSearchPlayer.Enumerator;
using PresenceSearchPlayer.Handler.CommandHandler.Error;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.CommandHandler
{
    public abstract class PSPCommandHandlerBase : CommandHandlerBase
    {
        protected GPErrorCode _errorCode;
        /// <summary>
        /// Be careful the return of query function should be List type,
        /// the decision formula should use _result.Count==0
        /// </summary>
        protected string _sendingBuffer;
        protected RequestModelBase _request;
        public PSPCommandHandlerBase(ISession client, Dictionary<string, string> recv) : base(client)
        {
            _errorCode = GPErrorCode.NoError;
        }

        public override void Handle()
        {
            base.Handle();

            CheckRequest();

            if (_errorCode < GPErrorCode.NoError)
            {
                return;
            }

            DataOperation();

            if (_errorCode < GPErrorCode.NoError)
            {
                return;
            }

            ConstructResponse();

            if (_errorCode < GPErrorCode.NoError)
            {
                return;
            }

            Response();
        }

        protected abstract void CheckRequest();

        protected abstract void DataOperation();

        /// <summary>
        /// The general message and error response should be writing in this child method.
        /// The base method only handles postfix adding and response validate checking.
        /// </summary>
        protected virtual void ConstructResponse()
        {
            if (!StringExtensions.CheckResponseValidation(_sendingBuffer))
            {
                return;
            }

            if (_request.OperationID != 0)
            {
                _sendingBuffer += $@"id\{_request.OperationID}\final\";
            }
            else
            {
                _sendingBuffer += @"\final\";
            }
        }

        protected virtual void Response()
        {
            if (!StringExtensions.CheckResponseValidation(_sendingBuffer))
            {
                return;
            }
            _session.SendAsync(_sendingBuffer);
        }
    }
}
