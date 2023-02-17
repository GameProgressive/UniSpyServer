using UniSpy.Server.PresenceSearchPlayer.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.AuthAdd
{
    public class GPAuthAddException : GPException
    {
        public GPAuthAddException() : base("The adding of authentication failed!", GPErrorCode.AuthAdd)
        {
        }

        public GPAuthAddException(string message) : base(message, GPErrorCode.AuthAdd)
        {
        }

        public GPAuthAddException(string message, System.Exception innerException) : base(message, GPErrorCode.AuthAdd, innerException)
        {
        }
    }
}