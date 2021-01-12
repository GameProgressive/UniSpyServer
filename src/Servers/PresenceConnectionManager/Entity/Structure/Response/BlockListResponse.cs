using System;
using System.Linq;
using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    internal class BlockListResponse : PCMResponseBase
    {
        protected new BlockListResult _result => (BlockListResult)base._result;

        public BlockListResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            // \bdy\< num in list >\list\< profileid list - comma delimited >\final\
            SendingBuffer = $@"\bdy\{_result.ProfileIdList.Count()}\list\";
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
