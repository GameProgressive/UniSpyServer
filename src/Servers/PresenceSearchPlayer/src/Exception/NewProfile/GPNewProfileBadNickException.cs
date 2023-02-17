using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.NewProfile
{
    public class GPNewProfileBadNickException : GPNewProfileException
    {
        public GPNewProfileBadNickException() : base("Nickname is invalid at creating new profile!", GPErrorCode.NewProfileBadNick)
        {
        }

        public GPNewProfileBadNickException(string message) : base(message, GPErrorCode.NewProfileBadNick)
        {
        }

        public GPNewProfileBadNickException(string message, System.Exception innerException) : base(message, GPErrorCode.NewProfileBadNick, innerException)
        {
        }
    }
}