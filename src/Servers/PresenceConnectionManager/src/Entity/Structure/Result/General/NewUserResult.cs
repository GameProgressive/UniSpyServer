using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class NewUserResult : ResultBase
    {
        public Users User;
        public Profiles Profile;
        public Subprofiles SubProfile;
        public NewUserResult() { }
    }
}
