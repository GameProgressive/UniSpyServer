using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Extensions;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Handler.CommandHandler.Error;
using System.Collections.Generic;


namespace PresenceSearchPlayer.Handler.CommandHandler
{
    public abstract class PSPCommandHandlerBase : CommandHandlerBase
    {
        protected GPError _errorCode;
        /// <summary>
        /// Be careful the return of query function should be List type,
        /// the decision formula should use _result.Count==0
        /// </summary>
        protected string _sendingBuffer;
        public PSPCommandHandlerBase(ISession session, Dictionary<string, string> recv) : base(session)
        {
            _errorCode = GPError.NoError;
        }

        public override void Handle()
        {
            base.Handle();

            RequestCheck();

            if (_errorCode < GPError.NoError)
            {
                ConstructResponse();
                Response();
                return;
            }

            DataOperation();

            if (_errorCode < GPError.NoError)
            {
                ConstructResponse();
                Response();
                return;
            }

            ConstructResponse();
            Response();
        }

        protected virtual void RequestCheck() { }
        protected virtual void DataOperation() { }

        /// <summary>
        /// Usually we do not need to override this function
        /// </summary>
        protected virtual void ConstructResponse()
        {
            if (_errorCode != GPError.NoError)
            {
                BuildErrorResponse();
            }
            else
            {
                BuildNormalResponse();
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

        /// <summary>
        /// Customize the error response string
        /// </summary>
        protected virtual void BuildErrorResponse()
        {
            _sendingBuffer = ErrorMsg.BuildGPErrorMsg(_errorCode);
        }

        protected virtual void BuildNormalResponse()
        { }
    }
}
