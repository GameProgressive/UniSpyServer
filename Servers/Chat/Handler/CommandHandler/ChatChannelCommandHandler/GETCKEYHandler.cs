using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Server;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class GETCKEYHandler : ChatCommandHandlerBase
    {
        new GETCKEY _cmd;
        ChatChannelBase _channel;
        ChatChannelUser _user;
        public GETCKEYHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (GETCKEY)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
            if (!_session.UserInfo.GetJoinedChannel(_cmd.ChannelName, out _channel))
            {
                _systemError = ChatError.Parse;
                return;
            }

            //check whether user1 will get user2's key value by single search
            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _systemError = ChatError.DataOperation;
                return;
            }
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
                _systemError = ChatError.IRCError;
                _ircErrorCode = IRCError.NoSuchNick;
                return;
            }
            GetUserKeyValue(user);
        }

        private void GetUserBFlagsOnly(ChatChannelUser user)
        {
            string flags = user.GetBFlagsString();

            _sendingBuffer +=
                user.BuildReply(ChatReply.GetCKey,
                $"* {_channel.Property.ChannelName} {user.UserInfo.NickName} {_cmd.Cookie} {flags}");
        //ChatCommandBase.BuildNumericRPL(ChatServer.ServerDomain,
        //    ChatResponseType.GetCKey,
        //    $"* {_channel.Property.ChannelName} {user.UserInfo.NickName} {_cmd.Cookie} {flags}", "");
        }
        private void GetUserKeyValue(ChatChannelUser user)
        {
            //we do not have key value so we do not construct getckey response
            if (user.UserKeyValue.Count == 0)
            {
                _systemError = ChatError.DataOperation;
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
                user.BuildReply(ChatReply.GetCKey,
                $"* {_channel.Property.ChannelName} {user.UserInfo.NickName} {_cmd.Cookie} {flags}");
            //ChatCommandBase.BuildNumericRPL(ChatServer.ServerDomain,
            //    ChatResponseType.GetCKey,
            //    $"* {_channel.Property.ChannelName} {user.UserInfo.NickName} {_cmd.Cookie} {flags}", "");
        }


        private void BuildGetCKeyEndMessage()
        {
            _sendingBuffer +=
                _user.BuildReply(ChatReply.EndGetCKey,
                $"* {_channel.Property.ChannelName} {_cmd.Cookie}",
                "End Of /GETCKEY.");
            //ChatCommandBase.BuildNumericRPL(ChatServer.ServerDomain,
            //ChatResponseType.EndGetCKey,
            //$"* {_channel.Property.ChannelName} {_cmd.Cookie}",
            //"End Of /GETCKEY.");
        }
    }
}
