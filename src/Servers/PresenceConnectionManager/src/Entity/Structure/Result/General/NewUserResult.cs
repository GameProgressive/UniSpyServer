using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class NewUserResult : ResultBase
    {
        public User User;
        public Profile Profile;
        public Subprofile SubProfile;
        public NewUserResult() { }
    }
}
