using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Handler.CommandHandler.Error;
using System.Collections.Generic;


namespace PresenceSearchPlayer.Abstraction.BaseClass
{
    public abstract class PSPCommandHandlerBase : CommandHandlerBase
    {
        protected GPError _errorCode;
        /// <summary>
        /// Be careful the return of query function should be List type,
        /// the decision formula should use _result.Count==0
        /// </summary>
        protected string _sendingBuffer;
        protected PSPRequestBase _request;
        public PSPCommandHandlerBase(ISession session, IRequest request) : base(session)
        {
            _errorCode = GPError.NoError;
            _request = (PSPRequestBase)request;
        }

        public override void Handle()
        {
            base.Handle();

            CheckRequest();

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

        protected virtual void CheckRequest() { }
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
