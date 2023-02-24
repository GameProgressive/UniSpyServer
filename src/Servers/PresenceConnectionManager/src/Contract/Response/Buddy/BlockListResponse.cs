using System.Linq;
using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Contract.Result;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Response
{
    public sealed class BlockListResponse : ResponseBase
    {
        private new BlockListResult _result => (BlockListResult)base._result;

        public BlockListResponse(RequestBase request, BlockListResult result) : base(request, result)
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
