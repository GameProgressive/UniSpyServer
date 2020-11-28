using UniSpyLib.Abstraction.Interface;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Structure.Packet.Request;

namespace ServerBrowser.Handler.CommandHandler
{
    /// <summary>
    /// No server list update option only get ip and host port for client
    /// so we do not need to implement server key, info, uniquevalue stuff
    /// </summary>
    public class NoServerListHandler : UpdateOptionHandlerBase
    {
        public NoServerListHandler(ServerListRequest request, ISession session, byte[] recv) : base(request, session, recv)
        {
        }
    }
}
