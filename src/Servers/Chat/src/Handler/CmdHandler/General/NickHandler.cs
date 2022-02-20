using System.Linq;
using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Exception.IRC.General;
using UniSpyServer.Servers.Chat.Entity.Structure;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.General
{
    [HandlerContract("NICK")]
    public sealed class NickHandler : CmdHandlerBase
    {
        private new NickRequest _request => (NickRequest)base._request;
        public NickHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();
            string newNickName = _request.NickName;
            if (Client.ClientPool.Values.Where(x => ((ClientInfo)x.Info).NickName == newNickName).Count() == 1)
            {
                throw new ChatIRCNickNameInUseException(
                    $"The nick name: {_request.NickName} is already in use",
                    _request.NickName,
                    newNickName);
            }
        }

        protected override void DataOperation()
        {
            _client.Info.NickName = _request.NickName;
        }
        protected override void ResponseConstruct()
        {
            _response = new NickResponse(_request, _result);
        }
    }
}
