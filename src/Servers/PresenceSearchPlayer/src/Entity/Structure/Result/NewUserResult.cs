using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result
{
    public sealed class NewUserResult : ResultBase
    {
        public User User;
        public Profile Profile;
        public Subprofile SubProfile;
        public NewUserResult() { }
    }
}
