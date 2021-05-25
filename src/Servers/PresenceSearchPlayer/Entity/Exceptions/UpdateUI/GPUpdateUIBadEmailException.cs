using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.UpdateUI
{
    public class GPUpdateUIBadEmailException : GPExceptionBase
    {
        public GPUpdateUIBadEmailException() : base("Email is invalid!")
        {
            ErrorCode = GPErrorCode.UpdateUIBadEmail;
        }

        public GPUpdateUIBadEmailException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.UpdateUIBadEmail;
        }

        public GPUpdateUIBadEmailException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.UpdateUIBadEmail;
        }
    }
}