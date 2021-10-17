using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Contract;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceConnectionManager.Entity.Structure.Result
{
    internal sealed class NewUserResult : ResultBase
    {
        public Users User;
        public Profiles Profile;
        public Subprofiles SubProfile;
        public NewUserResult() { }
    }
}
