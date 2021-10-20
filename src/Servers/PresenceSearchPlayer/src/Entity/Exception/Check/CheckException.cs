using UniSpyServer.PresenceSearchPlayer.Entity.Enumerate;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Structure.Exception
{
    public class CheckException : GPException
    {
        public override string ErrorResponse => $@"\cur\{(uint)ErrorCode}\final\";
        public CheckException() : this("There was an error checking the user account.", GPErrorCode.Check)
        {
        }

        public CheckException(string message, GPErrorCode errorCode) : base(message, errorCode)
        {
        }

        public CheckException(string message, GPErrorCode errorCode, System.Exception innerException) : base(message, errorCode, innerException)
        {
        }

    }
}