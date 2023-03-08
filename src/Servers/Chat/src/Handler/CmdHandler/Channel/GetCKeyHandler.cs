using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Exception.IRC.General;
using UniSpy.Server.Chat.Aggregate.Misc.ChannelInfo;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Response.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Channel
{

    public sealed class GetCKeyHandler : ChannelHandlerBase
    {
        private new GetCKeyRequest _request => (GetCKeyRequest)base._request;
        private new GetCKeyResult _result { get => (GetCKeyResult)base._result; set => base._result = value; }

        public GetCKeyHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new GetCKeyResult();
        }


        protected override void DataOperation()
        {
            switch (_request.RequestType)
            {
                case GetKeyReqeustType.GetChannelAllUserKeyValue:
                    GetChannelAllUserKeyValue();
                    break;
                case GetKeyReqeustType.GetChannelSpecificUserKeyValue:
                    GetChannelSpecificUserKeyValue();
                    break;
            }
        }

        private void GetChannelAllUserKeyValue()
        {
            foreach (var user in _channel.Users.Values)
            {
                GetUserKeyValue(user);
            }
        }

        private void GetChannelSpecificUserKeyValue()
        {
            var user = _channel.GetChannelUser(_request.NickName);
            if (user is null)
            {
                throw new ChatIRCNoSuchNickException($"Can not find user with nickname:{_request.NickName} in channels.");
            }
            GetUserKeyValue(user);
        }

        private void GetUserKeyValue(ChannelUser user)
        {
            // we get user's values
            string userValues = user.KeyValues.GetValueString(_request.Keys);
            if (userValues == "")
            {
                return;
            }
            var model = new GetCKeyDataModel
            {
                NickName = user.Info.NickName,
                UserValues = userValues
            };
            _result.DataResults.Add(model);
        }

        protected override void ResponseConstruct()
        {
            _response = new GetCKeyResponse(_request, _result);
        }
    }
}
