using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.General;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{

    internal sealed class GETCKEYHandler : ChatChannelHandlerBase
    {
        private new GETCKEYRequest _request => (GETCKEYRequest)base._request;
        private new GETCKEYResult _result
        {
            get => (GETCKEYResult)base._result;
            set => base._result = value;
        }

        public GETCKEYHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new GETCKEYResult();
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
            ChatChannelUser user = _channel.GetChannelUserByNickName(_request.NickName);
            if (user == null)
            {
                _result.ErrorCode = ChatErrorCode.IRCError;
                return;
            }
            GetUserKeyValue(user);
        }

        private void GetUserKeyValue(ChatChannelUser user)
        {
            //we do not have key value so we do not construct getckey response
            if (user.UserKeyValue.Count == 0)
            {
                _result.ErrorCode = ChatErrorCode.DataOperation;
                return;
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
            _response = new GETCKEYResponse(_request, _result);
        }
    }
}
