using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result
{
    public sealed class NicksDataModel
    {
        public string NickName;
        public string UniqueNick;
    }

    public sealed class NicksResult : ResultBase
    {
        public List<NicksDataModel> DataBaseResults { get; set; }
        public bool IsRequireUniqueNicks { get; set; }
        public NicksResult()
        {
        }
    }
}
