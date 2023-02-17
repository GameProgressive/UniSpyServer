using UniSpy.Server.PresenceSearchPlayer.Enumerate;
namespace UniSpy.Server.PresenceSearchPlayer.Exception.General
{
    public class GPNotLoggedInException : GPException
    {
        public GPNotLoggedInException() : base("You are not logged in, please login first!", GPErrorCode.NotLoggedIn)
        {
        }

        public GPNotLoggedInException(string message) : base(message, GPErrorCode.NotLoggedIn)
        {
        }

        public GPNotLoggedInException(string message, System.Exception innerException) : base(message, GPErrorCode.NotLoggedIn, innerException)
        {
        }
    }
}