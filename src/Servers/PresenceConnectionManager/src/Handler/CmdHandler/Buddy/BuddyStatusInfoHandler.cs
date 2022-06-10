using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler.Buddy
{
    public sealed class BuddyStatusInfoHandler : CmdHandlerBase
    {
        // This is what the message should look like.  Its broken up for easy viewing.
        //
        // "\bsi\\state\\profile\\bip\\bport\\hostip\\hprivip\"
        // "\qport\\hport\\sessflags\\rstatus\\gameType\"
        // "\gameVnt\\gameMn\\product\\qmodeflags\"
        public BuddyStatusInfoHandler(IClient client, IRequest request) : base(client, request)
        {
            throw new System.NotImplementedException();
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }
    }
}
