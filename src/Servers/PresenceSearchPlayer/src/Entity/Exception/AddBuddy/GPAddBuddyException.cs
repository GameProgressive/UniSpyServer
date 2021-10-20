using UniSpyServer.PresenceSearchPlayer.Entity.Enumerate;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Exception.AddBuddy
{
    public class GPAddBuddyException : GPException
    {
        public GPAddBuddyException() : base("Unknown error occur at add buddy!", GPErrorCode.AddBuddy)
        {
        }

        public GPAddBuddyException(string message) : base(message, GPErrorCode.AddBuddy)
        {
        }

        public GPAddBuddyException(string message, System.Exception innerException) : base(message, GPErrorCode.AddBuddy, innerException)
        {
        }
    }
}