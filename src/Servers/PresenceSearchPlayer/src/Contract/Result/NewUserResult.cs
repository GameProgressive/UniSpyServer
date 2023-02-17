using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.Core.Database.DatabaseModel;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Result
{
    public sealed class NewUserResult : ResultBase
    {
        public User User;
        public Profile Profile;
        public Subprofile SubProfile;
        public NewUserResult() { }
    }
}
