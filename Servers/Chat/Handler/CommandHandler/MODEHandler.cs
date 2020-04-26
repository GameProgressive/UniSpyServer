using System.Linq;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class MODEHandler : ChatCommandHandlerBase
    {
        MODE _modeCmd;
        ChatChannelBase _channel;
        public MODEHandler(IClient client, ChatCommandBase cmd) : base(client, cmd)
        {
            _modeCmd = (MODE)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
            var result = _session.UserInfo.JoinedChannels.
                Where(c => c.Property.ChannelName == _modeCmd.ChannelName);
            if (result.Count() != 1)
            {
                _errorCode = ChatError.NoSuchChannel;
                return;
            }
            _channel = result.First();
        }

        public override void DataOperation()
        {
            base.DataOperation();

            switch (_modeCmd.RequestType)
            {
                case ModeRequestType.GetChannelModes:
                    string modes =
                _session.UserInfo.JoinedChannels.
                Where(c => c.Property.ChannelName == _modeCmd.ChannelName)
                .First().Property.ChannelMode.GetChannelMode();
                    _sendingBuffer = ChatCommandBase.BuildMessageRPL($"MODE {_modeCmd.ChannelName} {modes}", "");
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

        public override void ConstructResponse()
        {
            base.ConstructResponse();

            if (_errorCode > ChatError.NoError)
            {
                ChatCommandBase.BuildErrorRPL(_errorCode, "", "no channel found");
            }
        }
        private void ProcessOtherModeRequest()
        {
            //we check if the user is operator in channel
            ChatChannelUser user;
            if (!_channel.GetChannelUser(_session, out user))
            {
                _errorCode = ChatError.DataOperation;
                return;
            }

            if (!user.IsChannelOperator)
            {
                _errorCode = ChatError.DataOperation;
                return;
            }
            _channel.Property.SetProperties(user, _modeCmd);
        }

    }
}
