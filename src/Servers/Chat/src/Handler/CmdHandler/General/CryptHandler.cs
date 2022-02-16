using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Exception;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.General
{
    [HandlerContract("CRYPT")]
    public sealed class CryptHandler : CmdHandlerBase
    {
        private new CryptRequest _request => (CryptRequest)base._request;
        private new CryptResult _result{ get => (CryptResult)base._result; set => base._result = value; }
        // CRYPT des 1 gamename
        public CryptHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new CryptResult();
        }

        protected override void DataOperation()
        {
            string secretKey = DataOperationExtensions.GetSecretKey(_request.GameName);
            if (secretKey == null)
            {
                _session.Disconnect();
                throw new Exception("secret key not found.");
            }
            _session.UserInfo.GameSecretKey = secretKey;
            _session.UserInfo.GameName = _request.GameName;
            // 2. Prepare two keys
            ChatCrypt.Init(_session.UserInfo.ClientCTX, ChatConstants.ClientKey, _session.UserInfo.GameSecretKey);
            ChatCrypt.Init(_session.UserInfo.ServerCTX, ChatConstants.ServerKey, _session.UserInfo.GameSecretKey);
        }
        protected override void ResponseConstruct()
        {
            _response = new CryptResponse(_request, _result);
        }
        protected override void Response()
        {
            base.Response();
            _session.UserInfo.IsUsingEncryption = true;
        }
    }
}
