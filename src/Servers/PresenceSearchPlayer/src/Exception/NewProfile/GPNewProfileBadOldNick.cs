using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.NewProfile
{
    public class GPNewProfileBadOldNickException : GPNewProfileException
    {
        public GPNewProfileBadOldNickException() : base("There is an already exist nickname!", GPErrorCode.NewProfileBadOldNick)
        {
        }

        public GPNewProfileBadOldNickException(string message) : base(message, GPErrorCode.NewProfileBadOldNick)
        {
        }

        public GPNewProfileBadOldNickException(string message, System.Exception innerException) : base(message, GPErrorCode.NewProfileBadOldNick, innerException)
        {
        }
    }
}