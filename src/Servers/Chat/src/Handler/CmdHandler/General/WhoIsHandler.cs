using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Exception.IRC.General;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.General;
using UniSpy.Server.Chat.Contract.Result.General;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{

    public sealed class WhoIsHandler : CmdHandlerBase
    {
        private new WhoIsRequest _request => (WhoIsRequest)base._request;
        private new WhoIsResult _result { get => (WhoIsResult)base._result; set => base._result = value; }
        private ClientInfo _clientInfo;
        public WhoIsHandler(IClient client, IRequest request) : base(client, request){ }

        protected override void RequestCheck()
        {
            _result = new WhoIsResult();

            // there only existed one nick name
            base.RequestCheck();
            var clients = (ICollection<Client>)Client.ClientPool.Values;
            var client = clients.FirstOrDefault(x => x.Info.NickName == _request.NickName);

            if (client is null)
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
            _result.PublicIPAddress = _client.Connection.RemoteIPEndPoint.Address.ToString();
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
