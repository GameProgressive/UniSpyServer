using System.Linq;
using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Exception.IRC.General;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.General;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{
    
    public sealed class NickHandler : CmdHandlerBase
    {
        private new NickRequest _request => (NickRequest)base._request;
        public NickHandler(IClient client, IRequest request) : base(client, request){ }

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
