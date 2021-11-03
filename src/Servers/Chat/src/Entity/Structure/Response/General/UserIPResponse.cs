using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.General
{
    public sealed class UserIPResponse : ResponseBase
    {
        private new UserIPResult _result => (UserIPResult)base._result;
        public UserIPResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            SendingBuffer = BuildUserIPReply(_result.RemoteIPAddress);
        }
        public static string BuildUserIPReply(string ip)
        {
            return IRCReplyBuilder.Build(ResponseName.UserIP, null, $"@{ip}");
        }


    }
}
