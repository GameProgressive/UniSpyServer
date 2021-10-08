using PresenceSearchPlayer.Abstraction.BaseClass;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Entity.Structure.Result
{
    public class NicksDataModel
    {
        public string NickName;
        public string UniqueNick;
    }

    public class NicksResult : ResultBase
    {
        public List<NicksDataModel> DataBaseResults;
        public bool IsRequireUniqueNicks { get; set; }
        public NicksResult()
        {
            DataBaseResults = new List<NicksDataModel>();
        }
    }
}
