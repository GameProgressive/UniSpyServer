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
            ChatChannelBase channel;
            if (_session.UserInfo.JoinedChannels.Count == 0)
            {
                _errorCode = Entity.Structure.ChatError.DataOperation;
                return;
            }
            channel = _session.UserInfo.JoinedChannels.
                Where(c => c.Property.ChannelName == _partCmd.ChannelName).First();
            if (channel==null)
            {
                _errorCode = Entity.Structure.ChatError.DataOperation;
                return;
            }
            channel.LeaveChannel(_session,_partCmd.Reason);
        }
    }
}
