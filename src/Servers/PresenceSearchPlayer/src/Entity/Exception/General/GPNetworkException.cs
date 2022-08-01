using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General
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