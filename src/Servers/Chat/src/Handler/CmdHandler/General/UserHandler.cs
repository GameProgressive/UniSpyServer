using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.General
{
    [HandlerContract("USER")]
    public sealed class UserHandler : CmdHandlerBase
    {
        private new UserRequest _request => (UserRequest)base._request;
        public UserHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            _client.Info.UserName = _request.UserName;
            _client.Info.Name = _request.Name;
            _client.Info.IsLoggedIn = true;
        }
    }
}
