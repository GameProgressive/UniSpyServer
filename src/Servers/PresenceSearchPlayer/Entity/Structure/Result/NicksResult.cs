using PresenceSearchPlayer.Abstraction.BaseClass;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Entity.Structure.Result
{
    public class NicksDataBaseModel
    {
        public string NickName;
        public string UniqueNick;
    }

    public class NicksResult : PSPResultBase
    {
        public List<NicksDataBaseModel> DataBaseResults;
        public bool IsRequireUniqueNicks { get; set; }
        public NicksResult()
        {
            DataBaseResults = new List<NicksDataBaseModel>();
        }
    }
}
