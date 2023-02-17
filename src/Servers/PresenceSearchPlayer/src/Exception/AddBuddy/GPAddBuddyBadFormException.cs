using UniSpy.Server.PresenceSearchPlayer.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.AddBuddy
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