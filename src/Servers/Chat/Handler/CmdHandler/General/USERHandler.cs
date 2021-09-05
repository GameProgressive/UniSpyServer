using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Request.General;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    internal sealed class USERHandler : ChatCmdHandlerBase
    {
        private new USERRequest _request => (USERRequest)base._request;
        public USERHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
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
