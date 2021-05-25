using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.UpdatePro
{
    public class GPUpdateProException : GPExceptionBase
    {
        public GPUpdateProException() : base("Update profile unknown error!")
        {
            ErrorCode = GPErrorCode.UpdatePro;
        }

        public GPUpdateProException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.UpdatePro;
        }

        public GPUpdateProException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.UpdatePro;
        }
    }
}