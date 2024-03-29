using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.General;
using UniSpy.Server.Chat.Contract.Result.General;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{

    public sealed class UserIPHandler : CmdHandlerBase
    {

        private new UserIPRequest _request => (UserIPRequest)base._request;
        private new UserIPResult _result { get => (UserIPResult)base._result; set => base._result = value; }
        public UserIPHandler(IChatClient client, UserIPRequest request) : base(client, request){ }

        protected override void DataOperation()
        {
            _result = new UserIPResult();
            _result.RemoteIPAddress = _client.Connection.RemoteIPEndPoint.Address;

        }
        protected override void ResponseConstruct()
        {
            _response = new UserIPResponse(_request, _result);
        }
    }
}
