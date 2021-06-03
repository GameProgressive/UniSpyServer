using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.AuthAdd
{
    public class GPAuthAddBadSigException : GPExceptionBase
    {
        public GPAuthAddBadSigException() : base("The signature in authentication is invalid!")
        {
            ErrorCode = GPErrorCode.AuthAddBadSig;
        }

        public GPAuthAddBadSigException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.AuthAddBadSig;
        }

        public GPAuthAddBadSigException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.AuthAddBadSig;
        }
    }
}