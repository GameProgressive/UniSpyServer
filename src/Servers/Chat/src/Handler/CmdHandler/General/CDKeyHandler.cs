using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.General
{
    [HandlerContract("CDKEY")]
    public sealed class CDKeyHandler : CmdHandlerBase
    {
        private new CDKeyRequest _request => (CDKeyRequest)base._request;

        public CDKeyHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void ResponseConstruct()
        {
            _response = new CDKeyResponse(_request, _result);
        }
    }
}
