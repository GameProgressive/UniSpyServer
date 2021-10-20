using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.WebServer.Network;

namespace UniSpyServer.WebServer.Abstraction
{
    public abstract class CmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected new Session _session => (Session)base._session;
        protected CmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}
