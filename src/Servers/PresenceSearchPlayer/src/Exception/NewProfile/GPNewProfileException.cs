using UniSpy.Server.PresenceSearchPlayer.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.NewProfile
{
    public class GPNewProfileException : GPException
    {
        public GPNewProfileException() : base("An unknown error occur when creating new profile!", GPErrorCode.NewProfile)
        {
        }

        public GPNewProfileException(string message) : base(message, GPErrorCode.NewProfile)
        {
        }

        public GPNewProfileException(string message, System.Exception innerException) : base(message, GPErrorCode.NewProfile, innerException)
        {
        }

        public GPNewProfileException(string message, GPErrorCode errorCode) : base(message, errorCode)
        {
        }

        public GPNewProfileException(string message, GPErrorCode errorCode, System.Exception innerException) : base(message, errorCode, innerException)
        {
        }
    }
}