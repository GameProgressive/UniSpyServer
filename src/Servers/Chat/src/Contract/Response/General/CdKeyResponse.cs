using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Contract.Response.General
{
    public sealed class CdKeyResponse : ResponseBase
    {
        public CdKeyResponse(RequestBase request, ResultBase result) : base(request, result) { }

        public override void Build()
        {
            //CDKey is always true
            SendingBuffer = $":{ServerDomain} {ResponseName.CDKey} * 1 :Authenticated \r\n";
        }
    }
}
