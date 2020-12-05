using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Handler.CommandHandler.Error;
using System.Collections.Generic;
using PresenceConnectionManager.Network;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    /// <summary>
    /// Because all errors are sent by SendGPError()
    /// so we if the error code != noerror we send it
    /// </summary>
    public abstract class PCMCommandHandlerBase : CommandHandlerBase
    {
        protected GPErrorCode _errorCode;
        protected string _sendingBuffer;
        protected new PCMSession _session;
        protected PCMRequestBase _request;
        public PCMCommandHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session)
        {
            _errorCode = GPErrorCode.NoError;
            _request = (PCMRequestBase)request;
            _session = (PCMSession)session;
        }

        public override void Handle()
        {
            base.Handle();

            CheckRequest();

            if (_errorCode != GPErrorCode.NoError)
            {
                ConstructResponse();
                Response();
                return;
            }

            DataOperation();

            if (_errorCode != GPErrorCode.NoError)
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
        /// Usually we do not need to override this method
        /// </summary>
        protected virtual void ConstructResponse()
        {
            if (_errorCode != GPErrorCode.NoError)
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
            base._session.SendAsync(_sendingBuffer);
        }

        protected virtual void BuildErrorResponse()
        {
            _sendingBuffer = ErrorMsg.BuildGPErrorMsg(_errorCode);
        }

        protected virtual void BuildNormalResponse() { }
    }
}
