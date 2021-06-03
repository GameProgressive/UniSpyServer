using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.AuthAdd
{
    public class GPAuthAddException : GPExceptionBase
    {
        public GPAuthAddException() : base("The adding of authentication failed!")
        {
            ErrorCode = GPErrorCode.AuthAdd;
        }

        public GPAuthAddException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.AuthAdd;
        }

        public GPAuthAddException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.AuthAdd;
        }
    }
}