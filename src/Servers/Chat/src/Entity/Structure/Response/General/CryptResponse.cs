using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.General
{
    public sealed class CryptResponse : ResponseBase
    {
        private new CryptResult _result => (CryptResult)base._result;
        public CryptResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            var cmdParams = $"* {ChatCrypt.ClientKey} {ChatCrypt.ServerKey}";
            SendingBuffer = IRCReplyBuilder.Build(
                ResponseName.SecureKey, cmdParams);
        }
    }
}
