using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Handler.SystemHandler.ChannelManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class SETCKEYHandler : ChatCommandHandlerBase
    {
        ChatChannelUser _user;
        new SETCKEY _cmd;
        ChatChannelBase _channel;
        public SETCKEYHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (SETCKEY)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();

            if (_session.UserInfo.NickName != _cmd.NickName)
            {
                _errorCode = ChatError.Parse;
                return;
            }

            if (!_session.UserInfo.IsJoinedChannel(_cmd.ChannelName))
            {
                _errorCode = ChatError.Parse;
                return;
            }

            if (!ChatChannelManager.GetChannel(_cmd.ChannelName, out _channel))
            {
                _errorCode = ChatError.Parse;
                return;
            }

            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatError.Parse;
                return;
            }
        }

        public override void DataOperation()
        {
            base.DataOperation();
            if (_cmd.NickName != _user.UserInfo.NickName)
            {
                _errorCode = ChatError.DataOperation;
                return;
            }
            _user.UpdateUserKeyValue(_cmd.KeyValues);
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();

            BuildBCASTReply();
        }

        public override void Response()
        {
            if (_sendingBuffer == null || _sendingBuffer == "" || _sendingBuffer.Length < 3)
            {
                return;
            }
            _channel.MultiCast(_sendingBuffer);
        }

        private void BuildBCASTReply()
        {
            //we only broadcast the b_flags
            string flags = "";
            if (_cmd.KeyValues.ContainsKey("b_flags"))
            {
                flags += @"\" + "b_flags" + @"\" + _cmd.KeyValues["b_flags"];
            }

            //todo check the paramemter 
            _sendingBuffer =
            ChatCommandBase.BuildReply(
                  ChatReply.GetCKey,
                  $"* {_channel.Property.ChannelName} {_user.UserInfo.NickName} BCAST {flags}");
        }
    }
}
