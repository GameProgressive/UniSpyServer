using UniSpy.Server.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Entity.Exception.NewProfile
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