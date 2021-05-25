using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.Login
{
    public class GPLoginTicketExpiredException : GPExceptionBase
    {
        public GPLoginTicketExpiredException() : base("The login ticket have expired!")
        {
            ErrorCode = GPErrorCode.LoginTicketExpired;
        }

        public GPLoginTicketExpiredException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.LoginTicketExpired;
        }

        public GPLoginTicketExpiredException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.LoginTicketExpired;
        }
    }
}