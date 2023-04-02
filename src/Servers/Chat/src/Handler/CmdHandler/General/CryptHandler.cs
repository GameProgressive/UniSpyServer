using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.General;
using UniSpy.Server.Chat.Contract.Result.General;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Extension;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.Chat.Application;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{

    public sealed class CryptHandler : CmdHandlerBase
    {
        private new CryptRequest _request => (CryptRequest)base._request;
        private new CryptResult _result { get => (CryptResult)base._result; set => base._result = value; }
        // CRYPT des 1 gamename
        public CryptHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new CryptResult();
        }
        public override void Handle()
        {
            if (_client.Info.IsRemoteClient)
            {
                _client.LogDebug("Ignore remote client Crypt request");
                return;
            }
            base.Handle();
        }
        protected override void DataOperation()
        {
            // we do not use crypto for remote client
            var client = (Client)_client;
            string secretKey = DataOperationExtensions.GetSecretKey(_request.GameName);
            if (secretKey is null)
            {
                client.Connection.Disconnect();
                throw new Chat.Exception("secret key not found.");
            }
            client.Info.GameSecretKey = secretKey;
            client.Info.GameName = _request.GameName;
            // 2. Prepare two keys
            client.Crypto = new ChatCrypt(_client.Info.GameSecretKey);
        }
        protected override void ResponseConstruct()
        {
            _response = new CryptResponse(_request, _result);
        }

        protected override void Response()
        {
            _response.Build();
            _client.LogNetworkSending(_response.SendingBuffer);
            // we need to send plaintext response here
            _client.Connection.Send(_response.SendingBuffer);
        }
    }
}
