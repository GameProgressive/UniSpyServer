using UniSpy.Server.PresenceSearchPlayer.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.AddBuddy
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