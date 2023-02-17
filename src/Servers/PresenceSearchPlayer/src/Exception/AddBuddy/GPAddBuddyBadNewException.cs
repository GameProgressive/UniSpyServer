using UniSpy.Server.PresenceSearchPlayer.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.AddBuddy
{
    public class GPAddBuddyBadNewException : GPException
    {
        public GPAddBuddyBadNewException() : base("The buddy name provided is invalid!", GPErrorCode.AddBuddyBadNew)
        {
        }

        public GPAddBuddyBadNewException(string message) : base(message, GPErrorCode.AddBuddyBadNew)
        {
        }

        public GPAddBuddyBadNewException(string message, System.Exception innerException) : base(message, GPErrorCode.AddBuddyBadNew, innerException)
        {
        }
    }
}