using UniSpy.Server.Core.Extension;
using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.V1.Application;
using UniSpy.Server.ServerBrowser.V1.Contract.Request;

namespace UniSpy.Server.ServerBrowser.V1.Handler.CmdHandler
{
    public sealed class GameNameHandler : CmdHandlerBase
    {
        private new GameNameRequest _request => (GameNameRequest)base._request;
        public GameNameHandler(Client client, RequestBase request) : base(client, request)
        {
        }

        protected override void RequestCheck()
        {
            // todo create ICryptographic implementation here based on the enctype
            base.RequestCheck();
            _client.Info.GameSecretKey = DataOperationExtensions.GetSecretKey(_request.GameName);
            // we currently do not care about this, assume client is received our challenge
            // todo  we need to decode the EncKey here, which is sended when client connect to us
            // if (_request.EncKey != ClientInfo.EncKey)
            // {
            //     throw new ServerBrowser.V2.Exception("Game encryption key is not valid");
            // }
        }
        protected override void DataOperation()
        {

            switch (_request.EncType)
            {
                case EncryptionType.Plaintext:
                    break;
                case EncryptionType.Type1:
                    // _client.Crypto = xxx;
                    break;
                case EncryptionType.Type2:
                    // _client.Crypto = xxx;
                    break;
            }
        }
    }
}