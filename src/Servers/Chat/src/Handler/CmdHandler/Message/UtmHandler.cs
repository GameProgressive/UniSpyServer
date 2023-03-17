using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Contract.Response.Message;
using UniSpy.Server.Chat.Contract.Result.Message;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Message
{
    /// <summary>
    /// Under the table message, this message will not display in regular chat
    /// </summary>
    public sealed class UtmHandler : MessageHandlerBase
    {
        private new UtmRequest _request => (UtmRequest)base._request;
        private new UtmResult _result { get => (UtmResult)base._result; set => base._result = value; }
        public UtmHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new UtmResult();
        }

        protected override void ResponseConstruct()
        {
            _response = new UtmResponse(_request, _result);
        }

    }
}