using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Structure.Packet.Response;
using ServerBrowser.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Handler.CmdHandler
{
    /// <summary>
    /// No server list update option only get ip and host port for client
    /// so we do not need to implement server key, info, uniquevalue stuff
    /// </summary>
    internal sealed class ServerNetworkInfoListHandler : ServerListUpdateOptionHandlerBase
    {
        public ServerNetworkInfoListHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new ServerMainListResult();
        }

        protected override void DataOperation()
        {
            // because we only need ip and host port, so we do nothing here
        }

        protected override void ResponseConstruct()
        {
            _response = new ServerMainListResponse(_request, _result);
        }
    }
}
