using UniSpy.Server.Core.Extension;
using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.V1.Aggregate;
using UniSpy.Server.ServerBrowser.V1.Application;
using UniSpy.Server.ServerBrowser.V1.Contract.Request;

namespace UniSpy.Server.ServerBrowser.V1.Handler.CmdHandler
{
    public sealed class GameNameHandler : CmdHandlerBase
    {
        private new GameNameRequest _request => (GameNameRequest)base._request;
        public GameNameHandler(Client client, GameNameRequest request) : base(client, request)
        {
        }

        protected override void RequestCheck()
        {
            // todo create ICryptographic implementation here based on the enctype
            base.RequestCheck();
            _client.Info.GameSecretKey = DataOperationExtensions.GetSecretKey(_request.GameName);
            _client.Info.ValidateKey = _request.ValidateKey;
        }
        protected override void DataOperation()
        {
            switch (_request.EncType)
            {
                case EncryptionType.Plaintext:
                    break;
                case EncryptionType.Type1:
                    _client.Crypto = new Enctype1(_request.ValidateKey);
                    break;
                case EncryptionType.Type2:
                    _client.Crypto = new Enctype2(_client.Info.GameSecretKey);
                    break;
            }
        }

        protected override void Response()
        {
            base.Response();
            // reset the crypto object to null
            _client.Crypto = null;
        }
    }
}