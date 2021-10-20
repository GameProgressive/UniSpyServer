using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Structure.Request.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Chat.Handler.CmdHandler.General
{
    [HandlerContract("USER")]
    public sealed class UserHandler : CmdHandlerBase
    {
        private new UserRequest _request => (UserRequest)base._request;
        public UserHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            _session.UserInfo.UserName = _request.UserName;
            _session.UserInfo.Name = _request.Name;
            _session.UserInfo.IsLoggedIn = true;
        }
    }
}
