using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Handler
{
    [HandlerContract("LoginUniqueNick")]
    public class LoginUniqueNickHandler : CmdHandlerBase
    {
        protected new LoginUniqueNickRequest _request => (LoginUniqueNickRequest)base._request;
        protected new LoginResult _result { get => (LoginResult)base._result; set => base._result = value; }
        public LoginUniqueNickHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new LoginResult();
        }
        protected override void DataOperation()
        {
            using (var db = new UniSpyContext())
            {
                // var result = from u in db
            }
        }
    }
}