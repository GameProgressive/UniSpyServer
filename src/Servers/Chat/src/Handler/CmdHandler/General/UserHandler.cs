using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Structure.Request.General;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
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
