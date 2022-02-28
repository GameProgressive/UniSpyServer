using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.NewUser
{
    public class GPNewUserException : GPException
    {
        public GPNewUserException() : this("There was an unknown error creating user account.", GPErrorCode.NewUser)
        {
        }
        public GPNewUserException(string message) : base(message, GPErrorCode.NewUser)
        {
        }
        public GPNewUserException(string message, System.Exception innerException) : base(message, GPErrorCode.NewUser, innerException)
        {
        }

        public GPNewUserException(string message, GPErrorCode errorCode) : base(message, errorCode)
        {
        }

        public GPNewUserException(string message, GPErrorCode errorCode, System.Exception innerException) : base(message, errorCode, innerException)
        {
        }
        public override void Build()
        {
            SendingBuffer = $@"\nur\{(int)ErrorCode}\final\";
        }
    }
}