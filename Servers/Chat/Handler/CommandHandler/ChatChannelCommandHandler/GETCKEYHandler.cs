using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Entity.Structure.ChatResponse.ChatGeneralResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatChannelCommandHandler
{

    public class GETCKEYHandler : ChatChannelHandlerBase
    {
        new GETCKEYRequest _request;

        public GETCKEYHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new GETCKEYRequest(request.RawRequest);
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
            if (!_request.Parse())
            {
                _errorCode = ChatError.Parse;
                return;
            }
        }

        protected override void DataOperation()
        {
            base.DataOperation();

            switch (_request.RequestType)
            {
                case GetKeyType.GetChannelAllUserKeyValue:
                    GetChannelAllUserKeyValue();
                    break;
                case GetKeyType.GetChannelSpecificUserKeyValue:
                    GetChannelSpecificUserKeyValue();
                    break;
            }

            BuildGetCKeyEndMessage();
        }

        private void GetChannelAllUserKeyValue()
        {
            _sendingBuffer = "";
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
                _errorCode = ChatError.IRCError;
                return;
            }
            GetUserKeyValue(user);
        }

        private void GetUserBFlagsOnly(ChatChannelUser user)
        {
            string flags = user.GetBFlagsString();

            _sendingBuffer += GETCKEYReply.BuildGetCKeyReply(
                    user.UserInfo.NickName, _channel.Property.ChannelName,
                    _request.Cookie, flags);
        }

        private void GetUserKeyValue(ChatChannelUser user)
        {
            //we do not have key value so we do not construct getckey response
            if (user.UserKeyValue.Count == 0)
            {
                _errorCode = ChatError.DataOperation;
                return;
            }

            if (_request.Keys.Count == 1 && _request.Keys.Contains("b_flags"))
            {
                GetUserBFlagsOnly(user);
            }
            else
            {
                GetAllKeyValues(user);
            }
        }

        private void GetAllKeyValues(ChatChannelUser user)
        {
            string flags = user.GetUserValuesString(_request.Keys);

            //todo check the paramemter 
            _sendingBuffer +=
                GETCKEYReply.BuildGetCKeyReply(
                    user.UserInfo.NickName,
                    _channel.Property.ChannelName,
                    _request.Cookie, flags);
        }


        private void BuildGetCKeyEndMessage()
        {
            _sendingBuffer +=
                GETCKEYReply.BuildEndOfGetCKeyReply(
                  _channel.Property.ChannelName,
                    _request.Cookie);
        }
    }
}
