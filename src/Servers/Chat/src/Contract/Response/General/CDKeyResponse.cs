using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Contract.Response.General
{
    public sealed class CDKeyResponse : ResponseBase
    {
        public CDKeyResponse(RequestBase request, ResultBase result) : base(request, result){ }

        public override void Build()
        {
            //CDKey is always true
            var cmdParams = $"param1 1 :Authenticated";
            SendingBuffer = IRCReplyBuilder.Build(
                ResponseName.CDKey, cmdParams);
        }
    }
}
