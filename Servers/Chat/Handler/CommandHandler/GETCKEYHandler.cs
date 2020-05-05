using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
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
                _errorCode = ChatError.Parse;
                return;
            }

            //check whether user1 will get user2's key value by single search
            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatError.DataOperation;
                return;
            }
        }

        public override void DataOperation()
        {
            base.DataOperation();

            switch (_cmd.RequestType)
            {
                case GetKeyType.GetChannelUsersKeyValue:
                    GetChannelUsersKeyValue();
                    BuildGetCKeyEndMessage();
                    break;
                case GetKeyType.GetChannelUserKeyValue:
                    GetChannelUserKeyValue();
                    BuildGetCKeyEndMessage();
                    break;
            }

           
        }


        private void GetChannelUsersKeyValue()
        {
            _sendingBuffer = "";
            foreach (var user in _channel.Property.ChannelUsers)
            {
                GetUserKeyValue(user);
            }
        }
        private void GetChannelUserKeyValue()
        {
            GetUserKeyValue(_user);
        }
        private void GetUserKeyValue(ChatChannelUser user)
        {
            //we do not have key value so we do not construct getckey response
            if (user.UserKeyValue.Count == 0)
            {
                return;
            }

            string flags = "";
            foreach (var k in _cmd.Keys)
            {
                if (user.UserKeyValue.ContainsKey(k))
                {
                    flags += @"\" + user.UserInfo.UserName + @"\" + user.UserKeyValue[k];
                }
            }

            //todo check the paramemter 
            _sendingBuffer +=
          ChatCommandBase.BuildNumericRPL(ChatServer.ServerDomain,
              ChatResponseType.GetCKey,
              $"* {_channel.Property.ChannelName} {user.UserInfo.NickName} {_cmd.Cookie} {flags}", "");
        }

        private void BuildGetCKeyEndMessage()
        {
            _sendingBuffer +=
                ChatCommandBase.BuildNumericRPL(ChatServer.ServerDomain,
                ChatResponseType.EndGetCKey,
                $"* {_channel.Property.ChannelName} {_cmd.Cookie}",
                "End Of /GETCKEY.");
        }
    }
}
