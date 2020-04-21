using System;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChannelManage;
using Chat.Server;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class NAMESHandler : ChatCommandHandlerBase
    {
        NAMES _namesCmd;
        ChatChannelBase _channel;
        public NAMESHandler(IClient client, ChatCommandBase cmd) : base(client, cmd)
        {
            _namesCmd = (NAMES)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
        }

        public override void ConstructResponse()
        {
            _channel.SendChannelUsers(_session);

            base.ConstructResponse();
        }

        public override void DataOperation()
        {
            ChatChannelManager.Channels.TryGetValue(_namesCmd.ChannelName, out _channel);
            if (_channel == null)
            {
                _errorCode = Entity.Structure.ChatError.DataOperation;
            }
            base.DataOperation();
        }
    }
}
