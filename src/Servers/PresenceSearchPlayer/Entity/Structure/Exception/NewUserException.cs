using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Exception
{
    public class NewUserException : GPExceptionBase
    {
        public override string ErrorResponse => $@"\nur\{ErrorCode}\final\";
        public NewUserException() : this("There was an unknown error creating user account.", GPErrorCode.NewUser)
        {
        }

        public NewUserException(string message, GPErrorCode errorCode) : base(message, errorCode)
        {
        }

        public NewUserException(string message, GPErrorCode errorCode, System.Exception innerException) : base(message, errorCode, innerException)
        {
        }
    }
}