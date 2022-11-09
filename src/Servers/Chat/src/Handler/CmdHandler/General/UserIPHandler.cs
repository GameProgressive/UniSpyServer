using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.General
{
    
    public sealed class UserIPHandler : CmdHandlerBase
    {

        private new UserIPRequest _request => (UserIPRequest)base._request;
        private new UserIPResult _result { get => (UserIPResult)base._result; set => base._result = value; }
        public UserIPHandler(IClient client, IRequest request) : base(client, request){ }

        protected override void DataOperation()
        {
            _result = new UserIPResult();
            _result.RemoteIPAddress = _client.Connection.RemoteIPEndPoint.ToString();

        }
        protected override void ResponseConstruct()
        {
            _response = new UserIPResponse(_request, _result);
        }
    }
}
