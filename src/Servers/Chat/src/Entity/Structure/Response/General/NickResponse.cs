using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.General
{
    public sealed class NickResponse : ResponseBase
    {
        private new NickRequest _request => (NickRequest)base._request;
        public NickResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            SendingBuffer = BuildWelcomeReply(_request.NickName);
        }
        public static string BuildWelcomeReply(string nickName)
        {
            return IRCReplyBuilder.Build(
                ResponseName.Welcome,
                cmdParams: nickName,
                tailing: "Welcome to UniSpy!");
        }
    }
}
