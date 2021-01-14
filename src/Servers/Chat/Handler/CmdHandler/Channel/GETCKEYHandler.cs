using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Entity.Structure.Response.General;
using Chat.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{

    public class GETCKEYHandler : ChatChannelHandlerBase
    {
        protected new GETCKEYRequest _request
        {
            get { return (GETCKEYRequest)base._request; }
        }
        protected new GETCKEYResult _result
        {
            get { return (GETCKEYResult)base._result; }
            set { base._result = value; }
        }

        public GETCKEYHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }


        protected override void DataOperation()
        {
            base.DataOperation();
            _result = new GETCKEYResult(_channel.Property.ChannelName, _request.Cookie);
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
            ChatChannelUser user;
            if (!_channel.GetChannelUserByNickName(_request.NickName, out user))
            {
                _errorCode = ChatErrorCode.IRCError;
                return;
            }
            GetUserKeyValue(user);
        }

        private void GetUserKeyValue(ChatChannelUser user)
        {
            //we do not have key value so we do not construct getckey response
            if (user.UserKeyValue.Count == 0)
            {
                _errorCode = ChatErrorCode.DataOperation;
                return;
            }

            if (_request.Keys.Count == 1 && _request.Keys.Contains("b_flags"))
            {
                // we get user's BFlag
                _result.NickNamesAndBFlags.Add(user.UserInfo.NickName, user.BFlags);
            }
            else
            {
                // we get user's values
                string userValues = user.GetUserValues(_request.Keys);
                _result.NickNamesAndBFlags.Add(user.UserInfo.NickName, userValues);
            }
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _response = new GETCKEYResponse(_result);
        }
    }
}
