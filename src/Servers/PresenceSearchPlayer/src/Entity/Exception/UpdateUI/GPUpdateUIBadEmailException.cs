using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.UpdateUI
{
    public class GPUpdateUIBadEmailException : GPUpdateUIException
    {
        public GPUpdateUIBadEmailException() : base("Email is invalid!", GPErrorCode.UpdateUIBadEmail)
        {
        }

        public GPUpdateUIBadEmailException(string message) : base(message, GPErrorCode.UpdateUIBadEmail)
        {
        }

        public GPUpdateUIBadEmailException(string message, System.Exception innerException) : base(message, GPErrorCode.UpdateUIBadEmail, innerException)
        {
        }
    }
}