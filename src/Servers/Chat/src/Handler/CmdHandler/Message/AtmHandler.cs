using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Contract.Response.Message;
using UniSpy.Server.Chat.Contract.Result.Message;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Message
{
    /// <summary>
    /// Above the table message handler
    /// </summary>
    public sealed class AtmHandler : MessageHandlerBase
    {
        new AtmRequest _request => (AtmRequest)base._request;
        new AtmResult _result{ get => (AtmResult)base._result; set => base._result = value; }
        public AtmHandler(IChatClient client, AtmRequest request) : base(client, request)
        {
            _result = new AtmResult();
        }
        protected override void ResponseConstruct()
        {
            _response = new AtmResponse(_request, _result);
        }
    }
}
