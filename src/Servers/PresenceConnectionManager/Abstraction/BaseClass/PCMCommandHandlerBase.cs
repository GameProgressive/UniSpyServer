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
    public abstract class PCMCommandHandlerBase : UniSpyCmdHandlerBase
    {
        protected GPErrorCode _errorCode;
        protected string _sendingBuffer;
        protected new PCMSession _session
        {
            get { return (PCMSession)base._session; }
        }
        protected new PCMRequestBase _request
        {
            get { return (PCMRequestBase)base._request; }
        }
        public PCMCommandHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session,request)
        {
            _errorCode = GPErrorCode.NoError;
        }

        public override void Handle()
        {
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


        /// <summary>
        /// Usually we do not need to override this method
        /// </summary>
        protected override void ConstructResponse()
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

        protected override void Response()
        {
            if (!StringExtensions.CheckResponseValidation(_sendingBuffer))
            {
                return;
            }
            base._session.SendAsync(_sendingBuffer);
        }

        protected override void BuildErrorResponse()
        {
            _sendingBuffer = ErrorMsg.BuildGPErrorMsg(_errorCode);
        }

    }
}
