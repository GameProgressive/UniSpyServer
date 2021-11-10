using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.Login
{
    public class GPLoginBadLoginTicketException : GPLoginException
    {
        public GPLoginBadLoginTicketException() : base("The login ticket is invalid!", GPErrorCode.LoginBadLoginTicket)
        {
        }

        public GPLoginBadLoginTicketException(string message) : base(message, GPErrorCode.LoginBadLoginTicket)
        {
        }

        public GPLoginBadLoginTicketException(string message, System.Exception innerException) : base(message, GPErrorCode.LoginBadLoginTicket, innerException)
        {
        }
    }
}