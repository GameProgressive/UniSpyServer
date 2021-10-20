using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Exception;
using UniSpyServer.Chat.Entity.Structure.Misc;
using UniSpyServer.Chat.Entity.Structure.Request.General;
using UniSpyServer.Chat.Entity.Structure.Response.General;
using UniSpyServer.Chat.Entity.Structure.Result.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Chat.Handler.CmdHandler.General
{
    [HandlerContract("CRYPT")]
    public sealed class CryptHandler : CmdHandlerBase
    {
        private new CryptRequest _request => (CryptRequest)base._request;
        private new CryptResult _result
        {
            get => (CryptResult)base._result;
            set => base._result = value;
        }
        // CRYPT des 1 gamename
        public CryptHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
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
