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
    internal sealed class NoServerListHandler : ServerListHandlerBase
    {
        public NoServerListHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new GeneralRequestResult();
        }
        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }
        protected override void ResponseConstruct()
        {
            _response = new GeneralRequestResponse(_request, _result);
        }
    }
}
