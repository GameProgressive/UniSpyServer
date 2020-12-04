using UniSpyLib.Abstraction.Interface;
using ServerBrowser.Abstraction.BaseClass;

namespace ServerBrowser.Handler.CommandHandler
{
    /// <summary>
    /// No server list update option only get ip and host port for client
    /// so we do not need to implement server key, info, uniquevalue stuff
    /// </summary>
    public class NoServerListHandler : UpdateOptionHandlerBase
    {
        public NoServerListHandler(ISession session, IRequest request) : base(session,request)
        {
        }
    }
}
