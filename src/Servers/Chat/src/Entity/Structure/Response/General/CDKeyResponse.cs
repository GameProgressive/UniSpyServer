using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.General
{
    public sealed class CDKeyResponse : ResponseBase
    {
        public CDKeyResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            //CDKey is always true
            var cmdParams = $"param1 1 :Authenticated";
            SendingBuffer = IRCReplyBuilder.Build(
                ResponseName.CDKey, cmdParams);
        }
    }
}
