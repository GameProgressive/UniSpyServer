using Chat.Entity.Structure.Misc;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Exception.IRC.General
{
    internal class IRCException : UniSpyException
    {
        public string ErrorCode { get; private set; }
        public virtual string ErrorResponse => IRCReplyBuilder.Build(ErrorCode);
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
    }
}
