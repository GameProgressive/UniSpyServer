using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.UpdateUI
{
    public class GPUpdateUIException : GPExceptionBase
    {
        public GPUpdateUIException() : base("Update user info unknown error!")
        {
            ErrorCode = GPErrorCode.UpdateUI;
        }

        public GPUpdateUIException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.UpdateUI;
        }

        public GPUpdateUIException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.UpdateUI;
        }
    }
}