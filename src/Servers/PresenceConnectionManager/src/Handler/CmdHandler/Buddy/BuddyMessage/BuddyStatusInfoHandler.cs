using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.PresenceConnectionManager.Abstraction.SystemHandler.Buddy
{
    public sealed class BuddyStatusInfoHandler : CmdHandlerBase
    {
        // This is what the message should look like.  Its broken up for easy viewing.
        //
        // "\bsi\\state\\profile\\bip\\bport\\hostip\\hprivip\"
        // "\qport\\hport\\sessflags\\rstatus\\gameType\"
        // "\gameVnt\\gameMn\\product\\qmodeflags\"
        public BuddyStatusInfoHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            throw new System.NotImplementedException();
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }
    }
}
