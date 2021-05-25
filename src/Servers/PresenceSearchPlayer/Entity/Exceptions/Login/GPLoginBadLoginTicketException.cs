using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.Login
{
    public class GPLoginBadLoginTicketException : GPExceptionBase
    {
        public GPLoginBadLoginTicketException() : base("The login ticket is invalid!")
        {
            ErrorCode = GPErrorCode.LoginBadLoginTicket;
        }

        public GPLoginBadLoginTicketException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.LoginBadLoginTicket;
        }

        public GPLoginBadLoginTicketException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.LoginBadLoginTicket;
        }
    }
}