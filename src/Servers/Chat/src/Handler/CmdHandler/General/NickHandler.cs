using System.Linq;
using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Error.IRC.General;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.General;
using UniSpy.Server.Chat.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{

    public sealed class NickHandler : CmdHandlerBase
    {
        private new NickRequest _request => (NickRequest)base._request;
        public NickHandler(IChatClient client, NickRequest request) : base(client, request) { }

        protected override void RequestCheck()
        {
            base.RequestCheck();
            int number = 0;
            string validNickName;
            var clientInfos = ClientManager.GetAllClientInfo();


            if(_request.NickName == "*")
            {
                validNickName = System.Guid.NewGuid().ToString();
                throw new NickNameInUseException(
                $"The nick name: {_request.NickName} is already in use",
                _request.NickName,
                validNickName);
            }
            if (ClientManager.GetAllClientInfo().Where(i => i.NickName == _request.NickName).Count() == 0)
            {
                _client.Info.NickName = _request.NickName;
            }
            else
            {
                while (true)
                {
                    string newNickName = _request.NickName + number;
                    if (ClientManager.GetAllClientInfo().Where(i => i.NickName == _request.NickName).Count() == 0)
                    {
                        validNickName = newNickName;
                        break;
                    }
                }
                throw new NickNameInUseException(
                $"The nick name: {_request.NickName} is already in use",
                _request.NickName,
                validNickName);
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
