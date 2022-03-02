using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Exception;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Extensions;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.General
{
    [HandlerContract("CRYPT")]
    public sealed class CryptHandler : CmdHandlerBase
    {
        private new CryptRequest _request => (CryptRequest)base._request;
        private new CryptResult _result { get => (CryptResult)base._result; set => base._result = value; }
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
                _client.Session.Disconnect();
                throw new ChatException("secret key not found.");
            }
            _client.Info.GameSecretKey = secretKey;
            _client.Info.GameName = _request.GameName;
            // 2. Prepare two keys
            _client.Crypto = new ChatCrypt(_client.Info.GameSecretKey);
        }
        protected override void ResponseConstruct()
        {
            _response = new CryptResponse(_request, _result);
        }

        protected override void Response()
        {
            _response.Build();
            LogWriter.LogNetworkSending(_client.Session.RemoteIPEndPoint, _response.SendingBuffer);
            // we need to send plaintext response here
            _client.Session.Send(_response.SendingBuffer);
        }
    }
}
