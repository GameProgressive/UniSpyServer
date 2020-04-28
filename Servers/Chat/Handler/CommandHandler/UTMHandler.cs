using System;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{

    public class UTMHandler : ChatCommandHandlerBase
    {
        public UTMHandler(IClient client, ChatCommandBase cmd) : base(client, cmd)
        {
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
        }

        public override void DataOperation()
        {
            base.DataOperation();
        }
    }
}
