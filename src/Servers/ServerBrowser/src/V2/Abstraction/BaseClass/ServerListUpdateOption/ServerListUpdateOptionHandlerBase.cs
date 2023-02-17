using System.Linq;
using UniSpy.Server.ServerBrowser.V2.Aggregate.Misc;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Extensions;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass
{
    public abstract class ServerListUpdateOptionHandlerBase : CmdHandlerBase
    {
        protected new ServerListUpdateOptionRequestBase _request => (ServerListUpdateOptionRequestBase)base._request;
        protected new ServerListUpdateOptionResultBase _result { get => (ServerListUpdateOptionResultBase)base._result; set => base._result = value; }
        protected new ServerListUpdateOptionResponseBase _response { get => (ServerListUpdateOptionResponseBase)base._response; set => base._response = value; }
        public ServerListUpdateOptionHandlerBase(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void RequestCheck()
        {
            base.RequestCheck();
            if (_client.Info.GameSecretKey is null)
            {
                string secretKey = DataOperationExtensions
                                .GetSecretKey(_request.GameName);
                //we first check and get secrete key from database
                if (secretKey is null)
                {
                    throw new System.ArgumentNullException("Can not find secretkey in database.");
                }
                //this is client public ip and default query port
                _client.Info.GameSecretKey = secretKey;
            }
            if (_client.Info.ClientChallenge is null)
            {
                _client.Info.ClientChallenge = _request.ClientChallenge;
            }
            // initialize sb encryption
            if (_client.Crypto is null)
            {
                _client.Crypto = new SBCrypt(
                    _client.Info.GameSecretKey,
                    _client.Info.ClientChallenge);
            }
        }

        protected override void Response()
        {
            _response.Build();
            var bodyBuffer = _response.SendingBuffer.Skip(14).ToArray();
            var headBuffer = _response.SendingBuffer.Take(14).ToArray();
            var bufferEncrypted = _client.Crypto.Encrypt(bodyBuffer);
            var buffer = headBuffer.Concat(bufferEncrypted).ToArray();
            _client.LogNetworkSending(_response.SendingBuffer);
            _client.Connection.Send(buffer);
        }
    }
}
