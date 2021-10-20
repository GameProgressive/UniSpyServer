using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Application;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Exception.IRC.General;
using UniSpyServer.Chat.Entity.Structure.Request.General;
using UniSpyServer.Chat.Entity.Structure.Response.General;
using UniSpyServer.Chat.Network;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Chat.Handler.CmdHandler.General
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
