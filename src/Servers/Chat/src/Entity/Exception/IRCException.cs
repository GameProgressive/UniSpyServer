using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Entity.Exception.IRC.General
{
    public class IRCException : UniSpyException, IResponse
    {
        public string ErrorCode { get; private set; }
        public virtual string ErrorResponse => IRCReplyBuilder.Build(ErrorCode);

        object IResponse.SendingBuffer => this.SendingBuffer;

        public object SendingBuffer { get; private set; }

        public IRCException()
        {
        }

        public IRCException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public IRCException(string message, string errorCode, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public void Build()
        {
            SendingBuffer = IRCReplyBuilder.Build(ErrorCode);
        }
    }
}
