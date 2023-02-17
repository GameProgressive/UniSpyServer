using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// currently we do not know how this work
    /// so we do not implement it
    /// </summary>
    
    public sealed class GetUdpRelayHandler : CmdHandlerBase
    {
        new GetUdpRelayRequest _request => (GetUdpRelayRequest)base._request;
        public GetUdpRelayHandler(IClient client, IRequest request) : base(client, request) { }
    }
}
