using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class PARTHandler : ChatCommandHandlerBase
    {
        new PART _cmd;
        ChatChannelBase _channel;
        ChatChannelUser _user;
        public PARTHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (PART)cmd;
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
        }

        public override void DataOperation()
        {
            base.DataOperation();

            if (_session.UserInfo.JoinedChannels.Count == 0)
            {
                _errorCode = ChatError.DataOperation;
                return;
            }

            if (!_session.UserInfo.GetJoinedChannel(_cmd.ChannelName, out _channel))
            {
                _errorCode = ChatError.DataOperation;
                return;
            }
          
            if (!_channel.GetChannelUser(_session, out _user))
            {
                return;
            }
 
            _channel.RemoveBindOnUserAndChannel(_user);

            _channel.MultiCastLeave(_user, _cmd.Reason);
        }
    }
}
