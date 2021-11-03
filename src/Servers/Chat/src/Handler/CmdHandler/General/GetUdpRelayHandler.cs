using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// currently we do not know how this work
    /// so we do not implement it
    /// </summary>
    [HandlerContract("GETUDPRELAY")]
    public sealed class GetUdpRelayHandler : CmdHandlerBase
    {
        new GetUdpRelayRequest _request => (GetUdpRelayRequest)base._request;
        public GetUdpRelayHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void DataOperation()
        {
        }
    }
}
