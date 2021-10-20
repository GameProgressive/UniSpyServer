using UniSpyServer.PresenceSearchPlayer.Entity.Enumerate;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Exception.AuthAdd
{
    public class GPAuthAddBadFormException : GPException
    {
        public GPAuthAddBadFormException() : base("The authentication is in bad form!", GPErrorCode.AuthAddBadForm)
        {
        }

        public GPAuthAddBadFormException(string message) : base(message, GPErrorCode.AuthAddBadForm)
        {
        }

        public GPAuthAddBadFormException(string message, System.Exception innerException) : base(message, GPErrorCode.AuthAddBadForm, innerException)
        {
        }
    }
}