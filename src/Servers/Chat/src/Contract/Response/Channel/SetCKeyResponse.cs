using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Channel;

namespace UniSpy.Server.Chat.Contract.Response.Channel
{
    public sealed class SetCKeyResponse : ResponseBase
    {
        private new SetCKeyRequest _request => (SetCKeyRequest)base._request;
        public SetCKeyResponse(SetCKeyRequest request) : base(request, null) { }
        public override void Build()
        {
            // the broadcast message must contain keys and values.
            SendingBuffer = $":{ServerDomain} {ResponseName.GetCKey} * {_request.ChannelName} {_request.NickName} {_request.Cookie} {_request.KeyValueString}\r\n";

            SendingBuffer += $":{ServerDomain} {ResponseName.EndGetCKey} * {_request.ChannelName} {_request.Cookie} :End Of SETCKEY.\r\n";
        }
    }
}
