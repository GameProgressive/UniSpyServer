using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class GETCKEYHandler : ChatJoinedChannelHandlerBase
    {
        new GETCKEY _cmd;

        public GETCKEYHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (GETCKEY)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();

            switch (_cmd.RequestType)
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
            if (!_channel.GetChannelUserByNickName(_cmd.NickName, out user))
            {
                _errorCode = ChatError.IRCError;
                return;
            }
            GetUserKeyValue(user);
        }

        private void GetUserBFlagsOnly(ChatChannelUser user)
        {
            string flags = user.GetBFlagsString();

            _sendingBuffer += ChatReply.BuildGetCKeyReply(
                    user, _channel.Property.ChannelName,
                    _cmd.Cookie, flags);
        }
        private void GetUserKeyValue(ChatChannelUser user)
        {
            //we do not have key value so we do not construct getckey response
            if (user.UserKeyValue.Count == 0)
            {
                _errorCode = ChatError.DataOperation;
                return;
            }

            if (_cmd.Keys.Count == 1 && _cmd.Keys.Contains("b_flags"))
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
            string flags = user.GetValuesString(_cmd.Keys);

            //todo check the paramemter 
            _sendingBuffer +=
                ChatReply.BuildGetCKeyReply(
                    user, _channel.Property.ChannelName,
                    _cmd.Cookie, flags);
        }


        private void BuildGetCKeyEndMessage()
        {
            _sendingBuffer +=
                ChatReply.BuildEndOfGetCKeyReply(
                    _user, _channel.Property.ChannelName,
                    _cmd.Cookie);
        }
    }
}
