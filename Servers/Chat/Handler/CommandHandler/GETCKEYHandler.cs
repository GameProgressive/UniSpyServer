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
        public GETCKEYHandler(IClient client, ChatCommandBase cmd) : base(client, cmd)
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
            if (!_channel.GetChannelUser(_session, out _user))
            {
                _errorCode = ChatError.Parse;
                return;
            }
        }

        public override void DataOperation()
        {
            base.DataOperation();
            switch (_cmd.RequestType)
            {
                case GetKeyType.GetAllChannelUserKeyValue:
                    GetAllChannelUserKeyValue();
                    break;
                case GetKeyType.GetChannelUserKeyValue:
                    GetUserKeyValue();
                    break;
            }

            BuildGetCKeyEndMessage();
        }


        private void GetAllChannelUserKeyValue()
        {
            _sendingBuffer = "";
            foreach(var user in _channel.Property.ChannelUsers)
            {
                string flags = "";
                foreach (var k in _cmd.Keys)
                {
                    if (user.UserKeyValue.ContainsKey(k))
                    {
                        flags += @"\" + k + @"\" + user.UserKeyValue[k];
                    }
                }
                flags += "\0";

                _sendingBuffer +=
              ChatCommandBase.BuildNormalRPL(
                  ChatResponseType.GetCKey,
                  $"{_channel.Property.ChannelName} {user.UserInfo.NickName} {_cmd.Cookie} {flags}", "");
            }
        }

        private void GetUserKeyValue()
        {
            string flags = "";
            foreach (var k in _cmd.Keys)
            {
                if (_user.UserKeyValue.ContainsKey(k))
                {
                    flags += @"\" + k + @"\" + _user.UserKeyValue[k];
                }
            }
            flags += "\0";

            _sendingBuffer +=
          ChatCommandBase.BuildNormalRPL(
              ChatResponseType.GetCKey,
              $"{_channel.Property.ChannelName} {_user.UserInfo.NickName} {_cmd.Cookie} {flags} param5", "");
        }

        private void BuildGetCKeyEndMessage()
        {
            _sendingBuffer +=
                ChatCommandBase.BuildNormalRPL(
                ChatResponseType.EndGetCKey,
                $"{_channel.Property.ChannelName} {_cmd.Cookie} param3 param4",
                "End Of GETCKEY.");
        }
    }
}
