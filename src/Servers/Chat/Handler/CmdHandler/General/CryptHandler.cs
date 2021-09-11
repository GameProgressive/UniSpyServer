using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Exception;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request.General;
using Chat.Entity.Structure.Response.General;
using Chat.Entity.Structure.Result.General;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;

namespace Chat.Handler.CmdHandler.General
{
    [HandlerContract("CRYPT")]
    internal sealed class CryptHandler : CmdHandlerBase
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
                throw new ChatException("secret key not found.");
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
