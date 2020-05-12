using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class MODEHandler : ChatJoinedChannelHandlerBase
    {
       new MODE _cmd;
        public MODEHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (MODE)cmd;
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
