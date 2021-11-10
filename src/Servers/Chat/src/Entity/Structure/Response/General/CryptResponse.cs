using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.General
{
    public sealed class CryptResponse : ResponseBase
    {
        private new CryptResult _result => (CryptResult)base._result;
        public CryptResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            var cmdParams = $"* {ChatConstants.ClientKey} {ChatConstants.ServerKey}";
            SendingBuffer = IRCReplyBuilder.Build(
                ResponseName.SecureKey, cmdParams);
        }
    }
}
