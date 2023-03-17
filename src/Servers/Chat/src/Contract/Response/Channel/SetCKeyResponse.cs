using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;

namespace UniSpy.Server.Chat.Contract.Response.Channel
{
    public sealed class SetCKeyResponse : ResponseBase
    {
        private new SetCKeyRequest _request => (SetCKeyRequest)base._request;
        private new SetCKeyResult _result => (SetCKeyResult)base._result;
        public SetCKeyResponse(SetCKeyRequest request, SetCKeyResult result) : base(request, result) { }
        public override void Build()
        {
            // the broadcast message must contain keys and values.
            SendingBuffer = $":{ServerDomain} {ResponseName.GetCKey} * {_result.ChannelName} {_result.NickName} {_request.Cookie} {_request.KeyValueString}\r\n";

            SendingBuffer += $":{ServerDomain} {ResponseName.EndGetCKey} * {_request.ChannelName} {_request.Cookie} :End Of SETCKEY.\r\n";
        }
    }
}
