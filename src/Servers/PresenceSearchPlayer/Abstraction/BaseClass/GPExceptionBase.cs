using System;
using PresenceSearchPlayer.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Abstraction.BaseClass
{
    public abstract class GPExceptionBase : UniSpyExceptionBase
    {
        public new GPErrorCode ErrorCode => (GPErrorCode)base.ErrorCode;

        public GPExceptionBase(string message, GPErrorCode errorCode) : base(message, errorCode)
        {
        }

        public GPExceptionBase(string message, GPErrorCode errorCode, Exception innerException) : base(message, errorCode, innerException)
        {
        }
    }
}