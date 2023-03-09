using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Result.General;

namespace UniSpy.Server.Chat.Contract.Response.General
{
    public sealed class PingResponse : ResponseBase
    {
        private new PingResult _result => (PingResult)base._result;
        public PingResponse(RequestBase request, ResultBase result) : base(request, result) { }

        public override void Build()
        {
            SendingBuffer = $":{_result.RequesterIRCPrefix} {ResponseName.Pong}\r\n";
        }
    }
}
