using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.UpdateUI
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