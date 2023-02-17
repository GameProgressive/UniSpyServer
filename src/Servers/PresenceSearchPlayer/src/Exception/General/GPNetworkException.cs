using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.General
{
    public class GPNetworkException : GPException
    {
        public GPNetworkException() : base("Unknown network error!", GPErrorCode.Network)
        {
        }

        public GPNetworkException(string message) : base(message, GPErrorCode.Network)
        {
        }

        public GPNetworkException(string message, System.Exception innerException) : base(message, GPErrorCode.Network, innerException)
        {
        }
    }
}