using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Extensions;
using PresenceConnectionManager.Entity.BaseClass;
using PresenceConnectionManager.Entity.Enumerator;
using PresenceConnectionManager.Handler.Error;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler
{
    /// <summary>
    /// Because all errors are sent by SendGPError()
    /// so we if the error code != noerror we send it
    /// </summary>
    public abstract class PCMCommandHandlerBase : CommandHandlerBase
    {
        protected GPErrorCode _errorCode;
        protected string _sendingBuffer;
        protected PCMRequestBase _request;
        new protected PCMSession _session;
        public PCMCommandHandlerBase(ISession session, Dictionary<string, string> recv) : base(session)
        {
            _errorCode = GPErrorCode.NoError;
            _session = (PCMSession)session.GetInstance();
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

        protected abstract void CheckRequest();

        protected abstract void DataOperation();

        protected virtual void ConstructResponse()
        {
            if (_errorCode != GPErrorCode.NoError)
            {
                BuildErrorMessage();
            }

            if (_request.OperationID != 0)
            {
                _sendingBuffer += $@"\{_request.OperationID}\final\";
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
            base._session.SendAsync(_sendingBuffer);
        }

        protected virtual void BuildErrorMessage()
        {
            _sendingBuffer = ErrorMsg.BuildGPErrorMsg(_errorCode);
        }
    }
}
