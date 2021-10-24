using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Entity.Structure.Response.Channel
{
    public sealed class SetCKeyResponse : ResponseBase
    {
        private new SetCKeyRequest _request => (SetCKeyRequest)base._request;
        private new SetCKeyResult _result => (SetCKeyResult)base._result;
        public SetCKeyResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            //we only broadcast the b_flags
            string flags = "";
            if (_request.KeyValues.ContainsKey("b_flags"))
            {
                flags += @"\" + "b_flags" + @"\" + _request.KeyValues["b_flags"];
            }

            //todo check the paramemter
            if (_result.IsSetOthersKeyValue)
            {
                SendingBuffer =
                    GetCKeyResponse.BuildGetCKeyReply(
                        _result.NickName,
                        _result.ChannelName,
                        "BCAST", flags);
            }
            else
            {
                SendingBuffer =
                    GetCKeyResponse.BuildGetCKeyReply(
                        _result.NickName,
                        _result.ChannelName,
                        "BCAST", flags); ;
            }
        }
    }
}
