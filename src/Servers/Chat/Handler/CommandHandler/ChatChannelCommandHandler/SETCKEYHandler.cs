using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Entity.Structure.ChatResponse.ChatGeneralResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatChannelCommandHandler
{
    // Sets channel key/values.
    // If user is NULL or "", the keys will be set on the channel.
    // Otherwise, they will be set on the user,
    // Only ops can set channel keys on other users.
    // Set a value to NULL or "" to clear that key.
    public class SETCKEYHandler : ChatChannelHandlerBase
    {
        new SETCKEYRequest _request;
        bool IsSetOthersKeyValue;
        ChatChannelUser _otherUser;
        public SETCKEYHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (SETCKEYRequest)request;
            IsSetOthersKeyValue = false;
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (_request.NickName != _session.UserInfo.NickName)
            {
                if (!_user.IsChannelOperator)
                {
                    _errorCode = ChatError.NotChannelOperator;
                    return;
                }
                IsSetOthersKeyValue = true;
                if (!_channel.GetChannelUserByNickName(_request.NickName, out _otherUser))
                {
                    _errorCode = ChatError.IRCError;
                    _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                    return;
                }
            }

        }

        protected override void DataOperation()
        {
            base.DataOperation();

            if (IsSetOthersKeyValue)
            {
                _otherUser.UpdateUserKeyValue(_request.KeyValues);
            }
            else
            {
                _user.UpdateUserKeyValue(_request.KeyValues);
            }
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            //we only broadcast the b_flags
            string flags = "";
            if (_request.KeyValues.ContainsKey("b_flags"))
            {
                flags += @"\" + "b_flags" + @"\" + _request.KeyValues["b_flags"];
            }

            //todo check the paramemter
            if (IsSetOthersKeyValue)
            {
                _sendingBuffer =
                    GETCKEYReply.BuildGetCKeyReply(
                        _otherUser.UserInfo.NickName,
                        _channel.Property.ChannelName,
                        "BCAST", flags);
            }
            else
            {
                _sendingBuffer =
                    GETCKEYReply.BuildGetCKeyReply(
                        _user.UserInfo.NickName,
                        _channel.Property.ChannelName,
                        "BCAST", flags);
            }
        }

        protected override void Response()
        {
            if (_sendingBuffer == null || _sendingBuffer == "" || _sendingBuffer.Length < 3)
            {
                return;
            }
            _channel.MultiCast(_sendingBuffer);
        }
    }
}
