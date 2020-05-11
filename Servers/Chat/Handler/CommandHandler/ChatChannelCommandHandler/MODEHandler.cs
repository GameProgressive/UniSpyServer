using System.Linq;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Handler.SystemHandler.ChannelManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class MODEHandler : ChatCommandHandlerBase
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
            if (!ChatChannelManager.GetChannel(_cmd.ChannelName, out _channel))
            {
                _errorCode = ChatError.IRCError;
                return;
            }
            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatError.IRCError;
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
            //_sendingBuffer = ChatCommandBase.BuildMessageRPL($"MODE {_cmd.ChannelName} {modes}", "");
            _sendingBuffer = _user.BuildReply(ChatReply.MODE, $"{_channel.Property.ChannelName} {modes}");
                //ChatCommandBase.BuildRPLWithoutMiddleTailing(
                //    _user.UserInfo, ChatRPL.MODE,
                //    $"{_channel.Property.ChannelName} {modes}");
        }

        private void ProcessOtherModeRequest()
        {
            //we check if the user is operator in channel
            ChatChannelUser user;
            if (!_channel.GetChannelUserBySession(_session, out user))
            {
                _errorCode = ChatError.DataOperation;
                return;
            }

            if (!user.IsChannelOperator)
            {
                _errorCode = ChatError.DataOperation;
                return;
            }
            _channel.Property.SetProperties(user, _cmd);
        }

    }
}
