using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Exception.IRC.General
{
    public class IRCException : UniSpyException, IResponse
    {
        public string ErrorCode { get; private set; }
        // public virtual string ErrorResponse => IRCReplyBuilder.Build(ErrorCode);

        object IResponse.SendingBuffer => this.SendingBuffer;

        public virtual string SendingBuffer { get; protected set; }

        public IRCException() { }

        public IRCException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public IRCException(string message, string errorCode, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public virtual void Build()
        {
            SendingBuffer = $":{Abstraction.BaseClass.ResponseBase.ServerDomain} {ErrorCode}\r\n";
        }
    }
}
