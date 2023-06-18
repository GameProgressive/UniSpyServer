using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Result.General;

namespace UniSpy.Server.Chat.Contract.Response.General
{
    public sealed class NickResponse : ResponseBase
    {
        private new NickResult _result => (NickResult)base._result;
        public NickResponse(RequestBase request, ResultBase result) : base(request, result) { }
        public override void Build()
        {
            SendingBuffer = $":{ServerDomain} {ResponseName.Welcome} {_result.NickName} :Welcome to UniSpy!\r\n";
        }
    }
}
