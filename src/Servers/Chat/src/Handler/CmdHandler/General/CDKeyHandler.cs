using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.General;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{
    
    public sealed class CDKeyHandler : CmdHandlerBase
    {
        private new CDKeyRequest _request => (CDKeyRequest)base._request;

        public CDKeyHandler(IClient client, IRequest request) : base(client, request){ }

        protected override void ResponseConstruct()
        {
            _response = new CDKeyResponse(_request, _result);
        }
    }
}
