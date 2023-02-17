using UniSpy.Server.PresenceSearchPlayer.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.UpdateUI
{
    public class GPUpdateUIException : GPException
    {
        public GPUpdateUIException() : base("Update user info unknown error!", GPErrorCode.UpdateUI)
        {
        }

        public GPUpdateUIException(string message) : base(message, GPErrorCode.UpdateUI)
        {
        }

        public GPUpdateUIException(string message, System.Exception innerException) : base(message, GPErrorCode.UpdateUI, innerException)
        {
        }

        public GPUpdateUIException(string message, GPErrorCode errorCode) : base(message, errorCode)
        {
        }

        public GPUpdateUIException(string message, GPErrorCode errorCode, System.Exception innerException) : base(message, errorCode, innerException)
        {
        }
    }
}