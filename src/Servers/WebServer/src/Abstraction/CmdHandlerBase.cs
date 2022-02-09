using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.Servers.WebServer.Network;

namespace UniSpyServer.Servers.WebServer.Abstraction
{
    public abstract class CmdHandlerBase : UniSpyLib.Abstraction.BaseClass.CmdHandlerBase
    {
        protected new Session _session => (Session)base._session;
        protected CmdHandlerBase(ISession session, IRequest request) : base(session, request)
        {
        }
    }
}
