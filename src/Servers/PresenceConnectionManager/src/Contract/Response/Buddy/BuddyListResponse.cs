using System.Linq;
using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Contract.Result;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Response
{
    public sealed class BuddyListResponse : ResponseBase
    {
        public BuddyListResponse(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
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
