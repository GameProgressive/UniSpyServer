using UniSpyServer.PresenceSearchPlayer.Entity.Enumerate;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Exception.AddBuddy
{
    public class GPAddBuddyBadFormException : GPException
    {
        public GPAddBuddyBadFormException() : base("Add buddy format invalid!", GPErrorCode.AddBuddyBadForm)
        {
        }

        public GPAddBuddyBadFormException(string message) : base(message, GPErrorCode.AddBuddyBadForm)
        {
        }

        public GPAddBuddyBadFormException(string message, System.Exception innerException) : base(message, GPErrorCode.AddBuddyBadForm, innerException)
        {
        }
    }
}