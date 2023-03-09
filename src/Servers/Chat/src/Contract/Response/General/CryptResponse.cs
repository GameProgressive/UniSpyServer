using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Result.General;

namespace UniSpy.Server.Chat.Contract.Response.General
{
    public sealed class CryptResponse : ResponseBase
    {
        private new CryptResult _result => (CryptResult)base._result;
        public CryptResponse(RequestBase request, ResultBase result) : base(request, result) { }
        public override void Build()
        {
            SendingBuffer = $":{ServerDomain} {ResponseName.SecureKey} * {ChatCrypt.ClientKey} {ChatCrypt.ServerKey}\r\n";
        }
    }
}
