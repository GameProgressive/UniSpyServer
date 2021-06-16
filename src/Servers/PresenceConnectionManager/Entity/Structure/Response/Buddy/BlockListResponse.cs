using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Result;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    internal sealed class BlockListResponse : PCMResponseBase
    {
        private new BlockListResult _result => (BlockListResult)base._result;

        public BlockListResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            // \blk\< num in list >\list\< profileid list - comma delimited >\final\
            SendingBuffer = $@"\blk\{_result.ProfileIdList.Count()}\list\";
            foreach (var pid in _result.ProfileIdList)
            {
                SendingBuffer += $@"{pid}";
                if (pid != _result.ProfileIdList.Last())
                {
                    SendingBuffer += ",";
                }
            }
            SendingBuffer += @"\final\";
        }
    }
}
