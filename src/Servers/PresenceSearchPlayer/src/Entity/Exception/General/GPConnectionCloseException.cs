using UniSpyServer.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Exception.General
{
    public class GPConnectionCloseException : GPException
    {
        public GPConnectionCloseException() : base("Client connection accidently closed!", GPErrorCode.ConnectionClose)
        {
        }

        public GPConnectionCloseException(string message) : base(message, GPErrorCode.ConnectionClose)
        {
        }

        public GPConnectionCloseException(string message, System.Exception innerException) : base(message, GPErrorCode.ConnectionClose, innerException)
        {
        }
    }
}