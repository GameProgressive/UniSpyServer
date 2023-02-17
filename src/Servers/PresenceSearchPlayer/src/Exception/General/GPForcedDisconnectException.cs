using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.General
{
    public class GPForcedDisconnectException : GPException
    {
        public GPForcedDisconnectException() : base("Client is forced to disconnect!", GPErrorCode.ForcedDisconnect)
        {
        }

        public GPForcedDisconnectException(string message) : base(message, GPErrorCode.ForcedDisconnect)
        {
        }

        public GPForcedDisconnectException(string message, System.Exception innerException) : base(message, GPErrorCode.ForcedDisconnect, innerException)
        {
        }
    }
}