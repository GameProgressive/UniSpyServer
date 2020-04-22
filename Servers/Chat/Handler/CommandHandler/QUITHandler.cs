using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class QUITHandler : ChatCommandHandlerBase
    {
        public QUITHandler(IClient client, ChatCommandBase cmd) : base(client, cmd)
        {
        }

        public override void DataOperation()
        {
            base.DataOperation();
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
        }


    }
}
