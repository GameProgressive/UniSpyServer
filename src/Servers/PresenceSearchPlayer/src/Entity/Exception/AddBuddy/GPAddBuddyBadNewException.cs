using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.AddBuddy
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