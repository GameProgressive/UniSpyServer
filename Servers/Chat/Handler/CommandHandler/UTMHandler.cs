using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{

    public class UTMHandler : ChatCommandHandlerBase
    {
        ChatChannelUser _user;
        ChatChannelBase _channel;
        new UTM _cmd;
        public UTMHandler(IClient client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (UTM)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
            switch (_cmd.RequestType)
            {
                case UTMCmdType.ChannelUTM:
                    if (!_session.UserInfo.GetJoinedChannel(_cmd.ChannelName,out _channel))
                    {
                        _errorCode = Entity.Structure.ChatError.Parse;
                        return;
                    }
                        break;
                case UTMCmdType.UserUTM:
                    break;

            }

        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
           // _sendingBuffer = ChatCommandBase.BuildMessageRPL($"UTM {}")
        }

        public override void DataOperation()
        {
            base.DataOperation();
        }
    }
}
