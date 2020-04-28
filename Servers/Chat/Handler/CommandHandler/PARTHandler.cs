using System;
using System.Linq;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class PARTHandler : ChatCommandHandlerBase
    {
        PART _partCmd;
        ChatChannelBase _channel;
        ChatChannelUser _user;
        public PARTHandler(IClient client, ChatCommandBase cmd) : base(client, cmd)
        {
            _partCmd = (PART)cmd;
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
                _errorCode = Entity.Structure.ChatError.DataOperation;
                return;
            }

            if (!_session.UserInfo.GetJoinedChannel(_partCmd.ChannelName, out _channel))
            {
                _errorCode = Entity.Structure.ChatError.DataOperation;
                return;
            }
          

            if (!_channel.GetChannelUser(_session, out _user))
            {
                _errorCode = Entity.Structure.ChatError.DataOperation;
                return;
            }
            _channel.LeaveChannel(_user, _partCmd.Reason);
        }
    }
}
