using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exception.Login
{
    public class GPLoginTicketExpiredException : GPLoginException
    {
        public GPLoginTicketExpiredException() : base("The login ticket have expired!", GPErrorCode.LoginTicketExpired)
        {
        }

        public GPLoginTicketExpiredException(string message) : base(message, GPErrorCode.LoginTicketExpired)
        {
        }

        public GPLoginTicketExpiredException(string message, System.Exception innerException) : base(message, GPErrorCode.LoginTicketExpired, innerException)
        {
        }
    }
}