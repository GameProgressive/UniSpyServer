using System.Collections.Generic;
using PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

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
        }

        public NicksResult(UniSpyRequestBase request) : base(request)
        {
            DataBaseResults = new List<NicksDataBaseModel>();
        }
    }
}
