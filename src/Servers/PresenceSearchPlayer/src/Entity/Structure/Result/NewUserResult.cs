using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Contract;
using UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result
{
    public sealed class NewUserResult : ResultBase
    {
        public Users User;
        public Profiles Profile;
        public Subprofiles SubProfile;
        public NewUserResult() { }
    }
}
