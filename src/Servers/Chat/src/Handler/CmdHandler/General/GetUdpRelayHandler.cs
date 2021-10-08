using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Structure.Request.General;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// currently we do not know how this work
    /// so we do not implement it
    /// </summary>
    [HandlerContract("GETUDPRELAY")]
    internal sealed class GetUdpRelayHandler : CmdHandlerBase
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
