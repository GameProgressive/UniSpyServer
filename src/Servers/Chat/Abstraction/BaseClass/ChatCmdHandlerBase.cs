using Chat.Entity.Exception.IRC.General;
using Chat.Entity.Structure.Response;
using Chat.Entity.Structure.Result;
using Chat.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Abstraction.BaseClass
{
    /// <summary>
    /// error code condition is complicated
    /// there are 2 types error code
    /// 1.irc numeric error code
    /// 2.self defined code
    /// we do not want to send self defined code, so if we find errorCode < noerror
    /// we just return.
    /// if error code bigger than noerror we need to process it in ConstructResponse()
    ///we also need to check the error code != noerror in ConstructResponse()
    /// </summary>
    internal abstract class ChatCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected new ChatRequestBase _request => (ChatRequestBase)base._request;
        protected new ChatResponseBase _response
        {
            get => (ChatResponseBase)base._response;
            set => base._response = value;
        }
        protected new ChatSession _session => (ChatSession)base._session;
        protected new ChatResultBase _result
        {
            get => (ChatResultBase)base._result;
            set => base._result = value;
        }


        public ChatCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        //if we use this structure the error response should also write to _sendingBuffer
        public override void Handle()
        {
            try
            {
                RequestCheck();
                DataOperation();
                ResponseConstruct();
                Response();
            }
            catch (ChatIRCException e)
            {
                _session.SendAsync(e.ErrorResponse);
            }

        }
        protected override void RequestCheck()
        {
        }
        protected override void ResponseConstruct()
        {
            _response = new ChatDefaultResponse(_request, _result);
        }
        protected override void Response()
        {
            if (_session.UserInfo.IsQuietMode)
            {
                return;
            }
            base.Response();
        }
    }
}
