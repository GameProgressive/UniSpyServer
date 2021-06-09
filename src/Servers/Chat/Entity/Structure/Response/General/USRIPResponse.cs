using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result.General;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    internal sealed class USRIPResponse : ChatResponseBase
    {
        private new USRIPResult _result => (USRIPResult)base._result;
        public USRIPResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
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
