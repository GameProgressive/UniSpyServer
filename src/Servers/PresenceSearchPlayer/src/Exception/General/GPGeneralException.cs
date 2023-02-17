using UniSpy.Server.PresenceSearchPlayer.Enumerate;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.General
{
    public class GPException : UniSpyException, IResponse
    {
        public GPErrorCode ErrorCode { get; private set; }
        public string SendingBuffer { get; protected set; }
        object IResponse.SendingBuffer => SendingBuffer;

        public GPException() : this("General Error!", GPErrorCode.General)
        {
        }

        public GPException(string message) : this(message, GPErrorCode.General)
        {
        }

        public GPException(string message, System.Exception innerException) : this(message, GPErrorCode.General, innerException)
        {
        }

        public GPException(string message, GPErrorCode errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public GPException(string message, GPErrorCode errorCode, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public virtual void Build()
        {
            SendingBuffer = $@"\error\\err\{(int)ErrorCode}\fatal\\errmsg\{this.Message}\final\";
        }
    }
}