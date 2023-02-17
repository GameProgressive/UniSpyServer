using UniSpy.Server.PresenceSearchPlayer.Entity.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Exception
{
    public class CheckException : GPException
    {
        public CheckException() : this("There was an error checking the user account.", GPErrorCode.Check)
        {
        }

        public CheckException(string message, GPErrorCode errorCode) : base(message, errorCode)
        {
        }

        public CheckException(string message, GPErrorCode errorCode, System.Exception innerException) : base(message, errorCode, innerException)
        {
        }
        public override void Build()
        {
            SendingBuffer = $@"\cur\{(int)ErrorCode}\final\"; ;
        }
    }
}