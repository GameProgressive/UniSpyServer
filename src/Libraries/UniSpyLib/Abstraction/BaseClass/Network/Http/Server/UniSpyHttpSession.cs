using NetCoreServer;
using UniSpyLib.Abstraction.Interface;

namespace UniSpyLib.Abstraction.BaseClass.Network.Http.Server
{
    public abstract class UniSpyHttpSession : HttpSession, IUniSpySession
    {
        protected UniSpyHttpSession(UniSpyHttpServer server) : base(server)
        {
        }
    }
}