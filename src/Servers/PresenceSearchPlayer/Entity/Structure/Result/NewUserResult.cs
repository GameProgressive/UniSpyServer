using PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceSearchPlayer.Entity.Structure.Result
{
    public class NewUserResult : PSPResultBase
    {
        public Users User;
        public Profiles Profile;
        public Subprofiles SubProfile;
        public NewUserResult() { }
    }
}
