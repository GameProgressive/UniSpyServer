using UniSpy.Server.PresenceSearchPlayer.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.AuthAdd
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