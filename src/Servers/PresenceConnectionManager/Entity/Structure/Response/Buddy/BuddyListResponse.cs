using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Result;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    internal sealed class BuddyListResponse : ResponseBase
    {
        public BuddyListResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        private new BuddyListResult _result => (BuddyListResult)base._result;

        public override void Build()
        {
            // \bdy\< num in list >\list\< profileid list - comma delimited >\final\
            SendingBuffer = $@"\bdy\{_result.ProfileIDList.Count()}\list\";
            foreach (var pid in _result.ProfileIDList)
            {
                SendingBuffer += $@"{pid}";
                if (pid != _result.ProfileIDList.Last())
                {
                    SendingBuffer += ",";
                }
            }
            SendingBuffer += @"\final\";
        }
    }
}
