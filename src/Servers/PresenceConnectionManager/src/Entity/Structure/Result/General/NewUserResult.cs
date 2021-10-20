using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql;

namespace UniSpyServer.PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class NewUserResult : ResultBase
    {
        public Users User;
        public Profiles Profile;
        public Subprofiles SubProfile;
        public NewUserResult() { }
    }
}
