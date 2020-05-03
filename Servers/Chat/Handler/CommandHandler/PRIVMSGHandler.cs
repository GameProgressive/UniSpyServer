using System.Net;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class PRIVMSGHandler : ChatCommandHandlerBase
    {
        new PRIVMSG _cmd;
        private ChatChannelBase _channel;
        private ChatChannelUser _user;
        public PRIVMSGHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (PRIVMSG)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
            if (!_session.UserInfo.GetJoinedChannel(_cmd.ChannelName, out _channel))
            {
                _errorCode = Entity.Structure.ChatError.Parse;
                return;
            }

            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = Entity.Structure.ChatError.Parse;
                return;
            }
        }

        public override void DataOperation()
        {
            base.DataOperation();
            if (!_channel.Property.ChannelMode.IsModeratedChannel)
            {
                return;
            }

            if (_channel.IsUserBanned(_user))
            {
                return;
            }

            if (!_user.IsVoiceable)
            {
                return;
            }
            if (_user.UserInfo.IsQuietMode)
            {
                return;
            }

        }
        public override void ConstructResponse()
        {
            base.ConstructResponse();
            if (_errorCode > Entity.Structure.ChatError.NoError)
            {
                _sendingBuffer = "";
            }
            else
            {
                string ip = ((IPEndPoint)_user.Session.Socket.RemoteEndPoint).Address.ToString();
                _sendingBuffer =
                    ChatCommandBase.BuildMessageRPL(
                        $"{_user.UserInfo.NickName}!{_user.UserInfo.UserName}@{ip}",
                        $"PRIVMSG {_channel.Property.ChannelName}", _cmd.Message);
            }
        }

        public override void Response()
        {
            if (_sendingBuffer == null || _sendingBuffer == "" || _sendingBuffer.Length < 3)
            {
                return;
            }
            _channel.MultiCastExceptSender(_user,_sendingBuffer);
            //_channel.MultiCast(_sendingBuffer);
        }

    }
}
