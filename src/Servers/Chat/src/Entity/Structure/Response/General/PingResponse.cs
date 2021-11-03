using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.General
{
    public sealed class PingResponse : ResponseBase
    {
        private new PingResult _result => (PingResult)base._result;
        public PingResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = IRCReplyBuilder.Build(_result.RequesterIRCPrefix, ResponseName.Pong, null, null);
        }
    }
}
