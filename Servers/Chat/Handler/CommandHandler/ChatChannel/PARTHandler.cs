using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class PARTHandler : ChatChannelHandlerBase
    {
        new PART _cmd;
        public PARTHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (PART)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();

            _channel.LeaveChannel(_user, _cmd.Reason);
        }
    }
}
