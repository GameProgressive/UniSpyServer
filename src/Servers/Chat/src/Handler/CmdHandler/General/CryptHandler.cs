using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.General;
using UniSpy.Server.Chat.Contract.Result.General;
using UniSpy.Server.Core.Extension;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{

    public sealed class CryptHandler : CmdHandlerBase
    {
        private new CryptRequest _request => (CryptRequest)base._request;
        private new CryptResult _result { get => (CryptResult)base._result; set => base._result = value; }
        private ICryptography _crypto;
        // CRYPT des 1 gamename
        public CryptHandler(IChatClient client, CryptRequest request) : base(client, request)
        {
            _result = new CryptResult();
        }
        protected override void DataOperation()
        {
            // we do not use crypto for remote client
            if (!_client.Info.IsRemoteClient)
            {
                string secretKey = DataOperationExtensions.GetSecretKey(_request.GameName);
                if (secretKey is null)
                {
                    (_client as Client)?.Connection.Disconnect();
                    throw new Chat.Exception("secret key not found.");
                }
                _client.Info.GameSecretKey = secretKey;
                // 2. Prepare two keys
                _crypto = new ChatCrypt(_client.Info.GameSecretKey);
            }
            _client.Info.GameName = _request.GameName;
        }
        protected override void ResponseConstruct()
        {
            _response = new CryptResponse(_request, _result);
        }

        protected override void Response()
        {
            base.Response();
            if (!_client.Info.IsRemoteClient)
            {
                ((Client)_client).Crypto = _crypto;
            }
        }
    }
}
