using System;
using System.Linq;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class MODEHandler:ChatCommandHandlerBase
    {
        MODE _modeCmd;
        public MODEHandler(IClient client, ChatCommandBase cmd) : base(client, cmd)
        {
            _modeCmd = (MODE)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
        }

        public override void ConstructResponse()
        {
            string modes =
                _session.ClientInfo.JoinedChannels.
                Where(c => c.Property.ChannelName == _modeCmd.ChannelName)
                .First().Property.ChannelMode.GetChannelMode();
            _sendingBuffer = _modeCmd.GenerateResponse(modes);
            base.ConstructResponse();
        }

        public override void DataOperation()
        {
            base.DataOperation();
        }
    }
}
