using UniSpyServer.PresenceSearchPlayer.Entity.Enumerate;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Exception.AuthAdd
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