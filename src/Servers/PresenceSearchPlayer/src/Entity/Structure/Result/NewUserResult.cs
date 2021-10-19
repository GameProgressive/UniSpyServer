using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Contract;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceSearchPlayer.Entity.Structure.Result
{
    public sealed class NewUserResult : ResultBase
    {
        public Users User;
        public Profiles Profile;
        public Subprofiles SubProfile;
        public NewUserResult() { }
    }
}
