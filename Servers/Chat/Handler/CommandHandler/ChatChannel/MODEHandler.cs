using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class MODEHandler : ChatLogedInHandlerBase
    {
       new MODE _cmd;
        ChatChannelBase _channel;
        ChatChannelUser _user;
        public MODEHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (MODE)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
            if (_errorCode != ChatError.NoError)
            {
                return;
            }

            switch (_cmd.RequestType)
            {
                case ModeRequestType.EnableUserQuietFlag:
                case ModeRequestType.DisableUserQuietFlag:
                    //we do not need to find user and its channel here
                    break;
                default:
                    GetChannelAndUser();
                    break;
            }
           
        }
        private void GetChannelAndUser()
        {
            if (_session.UserInfo.JoinedChannels.Count == 0)
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_cmd.ChannelName);
                return;
            }

            if (!_session.UserInfo.GetJoinedChannelByName(_cmd.ChannelName, out _channel))
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_cmd.ChannelName);
                return;
            }

            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatError.Parse;
                _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                return;
            }
        }

        public override void DataOperation()
        {
            base.DataOperation();

            switch (_cmd.RequestType)
            {
                case ModeRequestType.GetChannelModes:
                    GetChannelModes();
                    break;
                case ModeRequestType.EnableUserQuietFlag:
                    _session.UserInfo.SetQuietModeFlag(true);
                    break;
                case ModeRequestType.DisableUserQuietFlag:
                    _session.UserInfo.SetQuietModeFlag(false);
                    break;
                default:
                    ProcessOtherModeRequest();
                    break;
            }
        }

        public void GetChannelModes()
        {
            string modes =
             _channel.Property.ChannelMode.GetChannelMode();

            _sendingBuffer =
                ChatReply.BuildModeReply(
                _user, _channel.Property.ChannelName, modes);
        }

        private void ProcessOtherModeRequest()
        {
            //we check if the user is operator in channel
            if (!_user.IsChannelOperator)
            {
                _errorCode = ChatError.DataOperation;
                return;
            }
            _channel.Property.SetProperties(_user, _cmd);
        }
    }
}
