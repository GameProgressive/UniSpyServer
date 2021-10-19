using Chat.Abstraction.BaseClass;
using Chat.Application;
using Chat.Entity.Contract;
using Chat.Entity.Exception.IRC.General;
using Chat.Entity.Structure.Request.General;
using Chat.Entity.Structure.Response.General;
using Chat.Network;
using System.Linq;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    [HandlerContract("NICK")]
    public sealed class NickHandler : CmdHandlerBase
    {
        private new NickRequest _request => (NickRequest)base._request;
        public NickHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();
            string newNickName = _request.NickName;
            uint count = 0;
            if (ServerFactory.Server.SessionManager.SessionPool.Values.
                   Where(s => ((Session)s).UserInfo.NickName == newNickName)
                   .Count() == 1)
            {
                while (true)
                {
                    if (ServerFactory.Server.SessionManager.SessionPool.Values.
                        Where(s => ((Session)s).UserInfo.NickName == newNickName)
                        .Count() == 1)
                    {
                        newNickName = $"{newNickName}{count++}";
                    }
                    else
                    {
                        break;
                    }
                }
                throw new ChatIRCNickNameInUseException(
                    $"The nick name: {_request.NickName} is already in use",
                    _request.NickName,
                    newNickName);
            }
        }

        protected override void DataOperation()
        {
            _session.UserInfo.NickName = _request.NickName;
        }
        protected override void ResponseConstruct()
        {
            _response = new NickResponse(_request, _result);
        }
    }
}
