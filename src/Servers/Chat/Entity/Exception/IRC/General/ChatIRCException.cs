using Chat.Entity.Structure.Misc;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Exception.IRC.General
{
    internal class ChatIRCException : UniSpyExceptionBase
    {
        public string ErrorCode { get; private set; }
        public virtual string ErrorResponse => ChatIRCReplyBuilder.Build(ErrorCode);
        public ChatIRCException()
        {
        }

        public ChatIRCException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public ChatIRCException(string message, string errorCode, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
