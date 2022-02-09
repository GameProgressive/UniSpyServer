using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Application;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Exception.IRC.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.General;
using UniSpyServer.Servers.Chat.Network;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.General
{
    [HandlerContract("NICK")]
    public sealed class NickHandler : CmdHandlerBase
    {
        private new NickRequest _request => (NickRequest)base._request;
        public NickHandler(ISession session, IRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();
            string newNickName = _request.NickName;
            int count = 0;
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
