using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Result.General;

namespace UniSpy.Server.Chat.Contract.Response.General
{
    public sealed class CryptResponse : ResponseBase
    {
        private new CryptResult _result => (CryptResult)base._result;
        public CryptResponse(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result){ }
        public override void Build()
        {
            var cmdParams = $"* {ChatCrypt.ClientKey} {ChatCrypt.ServerKey}";
            SendingBuffer = IRCReplyBuilder.Build(
                ResponseName.SecureKey, cmdParams);
        }
    }
}
