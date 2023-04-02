using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Enumerate;

using UniSpy.Server.GameStatus.Contract.Request;
using UniSpy.Server.GameStatus.Contract.Response;
using UniSpy.Server.GameStatus.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.GameStatus.Application;

namespace UniSpy.Server.GameStatus.Handler.CmdHandler
{
    /// <summary>
    /// Authenticate with partnerid or profileid
    /// because we are not gamespy
    /// so we do not check response string
    /// </summary>
    public sealed class AuthPlayerHandler : CmdHandlerBase
    {
        private new AuthPlayerRequest _request => (AuthPlayerRequest)base._request;
        private new AuthPlayerResult _result { get => (AuthPlayerResult)base._result; set => base._result = value; }
        public AuthPlayerHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new AuthPlayerResult();
        }
        protected override void DataOperation()
        {
            //search database for user's password
            //We do not store user's plaintext password, so we can not check this response

            switch (_request.RequestType)
            {
                case AuthMethod.PartnerIdAuth:
                    _client.Info.ProfileId = StorageOperation.Persistance.GetProfileId(_request.AuthToken);
                    break;
                case AuthMethod.ProfileIdAuth:
                    //even if we do not check response challenge
                    //we have to check the pid is in our databse
                    _client.Info.ProfileId = StorageOperation.Persistance.GetProfileId(_request.ProfileId);
                    break;
                case AuthMethod.CDkeyAuth:
                    _client.Info.ProfileId = StorageOperation.Persistance.GetProfileId(_request.CdKeyHash, _request.NickName);
                    break;
                default:
                    throw new GameStatus.Exception("Unknown AuthP request type.");
            }
            if (_client.Info.ProfileId is null)
            {
                throw new GameStatus.Exception("Can not find profileID");
            }
            _result.ProfileId = _client.Info.ProfileId;
            _client.Info.IsPlayerAuthenticated = true;
        }

        protected override void ResponseConstruct()
        {
            _response = new AuthPlayerResponse(_request, _result);
        }
    }
}
