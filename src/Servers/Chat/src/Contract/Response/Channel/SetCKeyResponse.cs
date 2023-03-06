using UniSpy.Server.Chat.Abstraction.BaseClass;
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
            //we only broadcast the key value string which contains b_*
            string flags = _request.KeyValueString;
            SendingBuffer = GetCKeyResponse.BuildGetCKeyReply(
                                    _result.NickName,
                                    _result.ChannelName,
                                    _request.Cookie,
                                     flags);
        }
    }
}
