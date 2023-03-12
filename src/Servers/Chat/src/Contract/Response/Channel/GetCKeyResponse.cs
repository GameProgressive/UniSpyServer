using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;

namespace UniSpy.Server.Chat.Contract.Response.Channel
{
    public sealed class GetCKeyResponse : ResponseBase
    {
        private new GetCKeyRequest _request => (GetCKeyRequest)base._request;
        private new GetCKeyResult _result => (GetCKeyResult)base._result;

        public GetCKeyResponse(GetCKeyRequest request, GetCKeyResult result) : base(request, result) { }
        
        public override void Build()
        {
            SendingBuffer = "";
            foreach (var data in _result.DataResults)
            {
                SendingBuffer += $":{ServerDomain} {ResponseName.GetCKey} * {_request.ChannelName} {data.NickName} {_request.Cookie} {data.UserValues}\r\n";
            }

            SendingBuffer += $":{ServerDomain} {ResponseName.EndGetCKey} * {_request.ChannelName} {_request.Cookie} :End Of GETCKEY.\r\n";
        }
    }
}
