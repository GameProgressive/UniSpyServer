using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.Status
{
    public class GPStatusException : GPExceptionBase
    {
        public GPStatusException() : base("Unknown error happen when processing player status")
        {
            ErrorCode = GPErrorCode.Status;
        }

        public GPStatusException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.Status;
        }

        public GPStatusException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.Status;
        }
    }
}