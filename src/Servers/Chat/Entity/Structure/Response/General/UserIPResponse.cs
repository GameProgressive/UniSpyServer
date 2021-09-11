using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result.General;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    internal sealed class UserIPResponse : ResponseBase
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
            return ChatIRCReplyBuilder.Build(ChatReplyName.UserIP, null, $"@{ip}");
        }


    }
}
