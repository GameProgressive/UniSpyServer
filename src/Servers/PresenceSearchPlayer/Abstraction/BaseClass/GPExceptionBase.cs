using System;
using PresenceSearchPlayer.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Abstraction.BaseClass
{
    public abstract class GPExceptionBase : UniSpyExceptionBase
    {
        public GPErrorCode ErrorCode { get; private set; }
        public abstract string ErrorResponse { get; }
        
        protected GPExceptionBase()
        {
        }
        public GPExceptionBase(string message, GPErrorCode errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public GPExceptionBase(string message, GPErrorCode errorCode, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}