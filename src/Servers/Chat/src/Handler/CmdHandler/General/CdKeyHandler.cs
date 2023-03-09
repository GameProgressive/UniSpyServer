using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.General;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{
    
    public sealed class CdKeyHandler : CmdHandlerBase
    {
        private new CdKeyRequest _request => (CdKeyRequest)base._request;

        public CdKeyHandler(IClient client, IRequest request) : base(client, request){ }

        protected override void ResponseConstruct()
        {
            _response = new CdKeyResponse(_request, _result);
        }
    }
}
