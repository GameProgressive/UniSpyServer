using UniSpyServer.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.PresenceSearchPlayer.Entity.Contract;
using UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Structure.Result
{
    public sealed class NewUserResult : ResultBase
    {
        public Users User;
        public Profiles Profile;
        public Subprofiles SubProfile;
        public NewUserResult() { }
    }
}
