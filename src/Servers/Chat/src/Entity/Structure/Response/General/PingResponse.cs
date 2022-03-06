using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.General
{
    public sealed class PingResponse : ResponseBase
    {
        private new PingResult _result => (PingResult)base._result;
        public PingResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result){ }

        public override void Build()
        {
            SendingBuffer = IRCReplyBuilder.Build(_result.RequesterIRCPrefix, ResponseName.Pong, null, null);
        }
    }
}
