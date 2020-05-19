using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    // Sets channel key/values.
    // If user is NULL or "", the keys will be set on the channel.
    // Otherwise, they will be set on the user,
    // Only ops can set channel keys on other users.
    // Set a value to NULL or "" to clear that key.
    public class SETCKEYHandler : ChatChannelHandlerBase
    {
        new SETCKEY _cmd;
        bool IsSetOthersKeyValue;
        ChatChannelUser _otherUser;
        public SETCKEYHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (SETCKEY)cmd;
            IsSetOthersKeyValue = false;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
            if (_errorCode != ChatError.NoError)
            {
                return;
            }

            if (_cmd.NickName != _session.UserInfo.NickName)
            {
                if (!_user.IsChannelOperator)
                {
                    _errorCode = ChatError.NotChannelOperator;
                    return;
                }
                IsSetOthersKeyValue = true;
                if (!_channel.GetChannelUserByNickName(_cmd.NickName, out _otherUser))
                {
                    _errorCode = ChatError.IRCError;
                    _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                    return;
                }
            }

        }

        public override void DataOperation()
        {
            base.DataOperation();
 
            if (IsSetOthersKeyValue)
            {
                _otherUser.UpdateUserKeyValue(_cmd.KeyValues);
            }
            else
            {
                _user.UpdateUserKeyValue(_cmd.KeyValues);
            }
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
            if (IsSetOthersKeyValue)
            {
                _sendingBuffer = ChatReply.BuildGetCKeyReply(_otherUser, _channel.Property.ChannelName, "BCAST", flags);
            }
            else
            {
                _sendingBuffer = ChatReply.BuildGetCKeyReply(_user, _channel.Property.ChannelName, "BCAST", flags);
            }
      
          
        }
    }
}
