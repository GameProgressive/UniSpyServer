using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using WebServer.Network;

namespace WebServer.Abstraction
{
    internal abstract class CmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected new Session _session => (Session)base._session;
        protected CmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}
