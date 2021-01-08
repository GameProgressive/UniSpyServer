using UniSpyLib.Abstraction.Interface;
using PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;

namespace PresenceConnectionManager.Abstraction.SystemHandler.Buddy
{
    public class BuddyStatusInfoHandler : PCMCmdHandlerBase
    {
        // This is what the message should look like.  Its broken up for easy viewing.
        //
        // "\bsi\\state\\profile\\bip\\bport\\hostip\\hprivip\"
        // "\qport\\hport\\sessflags\\rstatus\\gameType\"
        // "\gameVnt\\gameMn\\product\\qmodeflags\"
        public BuddyStatusInfoHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }
    }
}
