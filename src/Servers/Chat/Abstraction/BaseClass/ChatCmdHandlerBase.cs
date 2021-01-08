using Chat.Entity.Structure;
using Chat.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using Serilog.Events;

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
    public abstract class ChatCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected ChatErrorCode _errorCode;
        protected new ChatRequestBase _request
        {
            get { return (ChatRequestBase)base._request; }
        }
        /// <summary>
        /// Generic response buffer
        /// if some handler have multiple response
        /// only use this for sending error message
        /// </summary>
        protected string _sendingBuffer;

        protected new ChatSession _session
        {
            get { return (ChatSession)base._session; }
        }

        protected new ChatResultBase _result
        {
            get { return (ChatResultBase)base._result; }
            set { base._result = value; }
        }

        public ChatCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session,request)
        {
            _errorCode = ChatErrorCode.NoError;
        }

        //if we use this structure the error response should also write to _sendingBuffer
        public override void Handle()
        {
            RequestCheck();

            if (_errorCode != ChatErrorCode.NoError)
            {
                if (_errorCode == ChatErrorCode.IRCError)
                {
                    ResponseConstruct();
                }
                return;
            }

            DataOperation();
            if (_errorCode != ChatErrorCode.NoError)
            {
                if (_errorCode == ChatErrorCode.IRCError)
                {
                    ResponseConstruct();
                }
                return;
            }

            ResponseConstruct();

            Response();
        }

        protected override void ResponseConstruct()
        {
            if (_errorCode != ChatErrorCode.NoError)
            {
                BuildErrorResponse();
            }
            else
            {
                BuildNormalResponse();
            }
        }

        protected override void BuildErrorResponse()
        {
            if (_errorCode != ChatErrorCode.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, $"{_errorCode} occured!");
            }
        }


        protected override void Response()
        {
            if (_sendingBuffer == null || _sendingBuffer == "" || _sendingBuffer.Length < 3)
            {
                return;
            }
            base._session.SendAsync(_sendingBuffer);
        }
    }
}
