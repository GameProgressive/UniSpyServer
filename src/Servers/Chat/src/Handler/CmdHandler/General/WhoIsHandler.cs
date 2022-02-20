using System.Collections.Generic;
using System.Linq;
using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Exception.IRC.General;
using UniSpyServer.Servers.Chat.Entity.Structure;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.General
{
    [HandlerContract("WHOIS")]
    public sealed class WhoIsHandler : CmdHandlerBase
    {
        private new WhoIsRequest _request => (WhoIsRequest)base._request;
        private new WhoIsResult _result { get => (WhoIsResult)base._result; set => base._result = value; }
        private ClientInfo _clientInfo;
        public WhoIsHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void RequestCheck()
        {
            _result = new WhoIsResult();

            // there only existed one nick name
            base.RequestCheck();
            var clients = (ICollection<Client>)Client.ClientPool.Values;
            var client = clients.Where(x=>x.Info.NickName == _request.NickName).FirstOrDefault();
  
            if (client == null)
            {
                throw new ChatIRCNoSuchNickException($"Can not find user with nickname:{_request.NickName}.");
            }
            _clientInfo = client.Info;
        }
        protected override void DataOperation()
        {
            _result.NickName = _clientInfo.NickName;
            _result.Name = _clientInfo.Name;
            _result.UserName = _clientInfo.UserName;
            _result.PublicIPAddress = _clientInfo.RemoteIPEndPoint.Address.ToString();
            foreach (var channel in _clientInfo.JoinedChannels.Values)
            {
                _result.JoinedChannelName.Add(channel.Name);
            }
        }
        protected override void ResponseConstruct()
        {
            _response = new WhoIsResponse(_request, _result);
        }


    }
}
