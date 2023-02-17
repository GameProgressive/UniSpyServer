using UniSpy.Server.PresenceSearchPlayer.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.AuthAdd
{
    public class GPAuthAddBadSigException : GPException
    {
        public GPAuthAddBadSigException() : base("The signature in authentication is invalid!", GPErrorCode.AuthAddBadSig)
        {
        }

        public GPAuthAddBadSigException(string message) : base(message, GPErrorCode.AuthAddBadSig)
        {
        }

        public GPAuthAddBadSigException(string message, System.Exception innerException) : base(message, GPErrorCode.AuthAddBadSig, innerException)
        {
        }
    }
}