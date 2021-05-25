using System;
using PresenceSearchPlayer.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Abstraction.BaseClass
{
    public class GPExceptionBase : UniSpyExceptionBase
    {
        public new GPErrorCode ErrorCode
        {
            get => (GPErrorCode)base.ErrorCode;
            protected set => base.ErrorCode = value;
        }
        public GPExceptionBase() : base()
        {
            ErrorCode = GPErrorCode.NoError;
        }

        public GPExceptionBase(string message) : base(message)
        {
            ErrorCode = GPErrorCode.NoError;
        }

        public GPExceptionBase(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.NoError;
        }
    }

}

