using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.PresenceConnectionManager.Application;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Buddy
{
    public sealed class BuddyStatusInfoHandler : LoggedInCmdHandlerBase
    {
        // This is what the message should look like.  Its broken up for easy viewing.
        //
        // "\bsi\\state\\profile\\bip\\bport\\hostip\\hprivip\"
        // "\qport\\hport\\sessflags\\rstatus\\gameType\"
        // "\gameVnt\\gameMn\\product\\qmodeflags\"
        public BuddyStatusInfoHandler(Client client, IRequest request) : base(client, request)
        {
            throw new System.NotImplementedException();
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }
    }
}
