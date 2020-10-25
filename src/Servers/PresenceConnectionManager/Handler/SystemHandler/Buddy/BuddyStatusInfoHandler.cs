using GameSpyLib.Common.Entity.Interface;
using PresenceConnectionManager.Handler.CommandHandler;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.SystemHandler.Buddy
{
    public class BuddyStatusInfoHandler : PCMCommandHandlerBase
    {
        // This is what the message should look like.  Its broken up for easy viewing.
        //
        // "\bsi\\state\\profile\\bip\\bport\\hostip\\hprivip\"
        // "\qport\\hport\\sessflags\\rstatus\\gameType\"
        // "\gameVnt\\gameMn\\product\\qmodeflags\"
        public BuddyStatusInfoHandler(ISession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
        }

        protected override void DataOperation()
        {
            base.DataOperation();
        }
    }
}
