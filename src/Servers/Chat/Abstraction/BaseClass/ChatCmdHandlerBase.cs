using Chat.Entity.Structure;
using Chat.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;

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
            RequestCheck();

            if (_result.ErrorCode != ChatErrorCode.NoError)
            {
                ResponseConstruct();
                Response();
                return;
            }

            DataOperation();
            if (_result.ErrorCode != ChatErrorCode.NoError)
            {
                ResponseConstruct();
                Response();
                return;
            }

            ResponseConstruct();
            Response();
        }
        protected override void RequestCheck()
        {
        }

        protected override void Response()
        {
            if (_response == null)
            {
                return;
            }
            _response.Build();
            if (!StringExtensions.CheckResponseValidation((string)_response.SendingBuffer))
            {
                return;
            }
            base._session.SendAsync((string)_response.SendingBuffer);
        }
    }
}
