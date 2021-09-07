using PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceSearchPlayer.Entity.Structure.Result
{
    public class NewUserResult : ResultBase
    {
        public Users User;
        public Profiles Profile;
        public Subprofiles SubProfile;
        public NewUserResult() { }
    }
}
