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
    public abstract class ChatCommandHandlerBase : UniSpyCmdHandlerBase
    {
        protected ChatError _errorCode;
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

        public ChatCommandHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session,request)
        {
            _errorCode = ChatError.NoError;
        }

        //if we use this structure the error response should also write to _sendingBuffer
        public override void Handle()
        {
            base.Handle();

            CheckRequest();

            if (_errorCode != ChatError.NoError)
            {
                if (_errorCode == ChatError.IRCError)
                {
                    ConstructResponse();
                }
                return;
            }

            DataOperation();
            if (_errorCode != ChatError.NoError)
            {
                if (_errorCode == ChatError.IRCError)
                {
                    ConstructResponse();
                }
                return;
            }

            ConstructResponse();

            Response();
        }
        protected virtual void CheckRequest()
        { }
        protected virtual void DataOperation()
        { }
        protected virtual void ConstructResponse()
        {
            if (_errorCode != ChatError.NoError)
            {
                BuildErrorResponse();
            }
            else
            {
                BuildNormalResponse();
            }
        }

        protected virtual void BuildErrorResponse()
        {
            if (_errorCode != ChatError.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, $"{_errorCode} occured!");
            }
        }

        protected virtual void BuildNormalResponse()
        { }


        protected virtual void Response()
        {
            if (_sendingBuffer == null || _sendingBuffer == "" || _sendingBuffer.Length < 3)
            {
                return;
            }
            base._session.SendAsync(_sendingBuffer);
        }
    }
}
