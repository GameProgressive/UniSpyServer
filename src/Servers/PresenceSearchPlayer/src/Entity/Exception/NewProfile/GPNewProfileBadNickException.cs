using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.NewProfile
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