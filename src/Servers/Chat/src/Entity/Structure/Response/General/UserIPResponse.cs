using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.General
{
    public sealed class UserIPResponse : ResponseBase
    {
        private new UserIPResult _result => (UserIPResult)base._result;
        public UserIPResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result){ }
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
