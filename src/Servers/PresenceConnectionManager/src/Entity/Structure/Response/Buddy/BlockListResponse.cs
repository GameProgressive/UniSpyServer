using System.Linq;
using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response
{
    public sealed class BlockListResponse : ResponseBase
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
