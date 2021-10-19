using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Exception;
using Chat.Entity.Exception.IRC.General;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.General;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    [HandlerContract("GETCKEY")]
    public sealed class GetCKeyHandler : ChannelHandlerBase
    {
        private new GetCKeyRequest _request => (GetCKeyRequest)base._request;
        private new GetCKeyResult _result
        {
            get => (GetCKeyResult)base._result;
            set => base._result = value;
        }

        public GetCKeyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new GetCKeyResult();
        }


        protected override void DataOperation()
        {
            switch (_request.RequestType)
            {
                case GetKeyType.GetChannelAllUserKeyValue:
                    GetChannelAllUserKeyValue();
                    break;
                case GetKeyType.GetChannelSpecificUserKeyValue:
                    GetChannelSpecificUserKeyValue();
                    break;
            }
        }

        private void GetChannelAllUserKeyValue()
        {
            foreach (var user in _channel.Property.ChannelUsers)
            {
                GetUserKeyValue(user);
            }
        }

        private void GetChannelSpecificUserKeyValue()
        {
            ChannelUser user = _channel.GetChannelUserByNickName(_request.NickName);
            if (user == null)
            {
                throw new ChatIRCNoSuchNickException($"Can not find user with nickname:{_request.NickName} in channels.");
            }
            GetUserKeyValue(user);
        }

        private void GetUserKeyValue(ChannelUser user)
        {
            //we do not have key value so we do not construct getckey response
            if (user.UserKeyValue.Count == 0)
            {
                throw new Exception("User's key value are empty.");
            }

            if (_request.Keys.Count == 1 && _request.Keys.Contains("b_flags"))
            {
                GETCKEYDataModel model = new GETCKEYDataModel
                {
                    NickName = user.UserInfo.NickName,
                    UserValues = user.BFlags
                };
                // we get user's BFlag
                _result.DataResults.Add(model);
            }
            else
            {
                // we get user's values
                string userValues = user.GetUserValues(_request.Keys);
                GETCKEYDataModel model = new GETCKEYDataModel
                {
                    NickName = user.UserInfo.NickName,
                    UserValues = userValues
                };
                _result.DataResults.Add(model);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new GetCKeyResponse(_request, _result);
        }
    }
}
