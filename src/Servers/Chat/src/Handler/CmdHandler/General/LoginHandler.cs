using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.General;
using UniSpy.Server.Chat.Contract.Result.General;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{

    public sealed class LoginHandler : CmdHandlerBase
    {

        private new LoginRequest _request => (LoginRequest)base._request;
        private new LoginResult _result { get => (LoginResult)base._result; set => base._result = value; }
        public LoginHandler(IChatClient client, LoginRequest request) : base(client, request)
        {
            _result = new LoginResult();
        }

        protected override void RequestCheck()
        {
            /// TODO: Verify which games does send a GS encoded password and not md5
            //we decoded gamespy encoded password then get md5 of it 
            //_password = GameSpyUtils.DecodePassword(_request.PasswordHash);
            //_password = StringExtensions.GetMD5Hash(_password);
            base.RequestCheck();
        }

        protected override void DataOperation()
        {
            switch (_request.ReqeustType)
            {
                case LoginReqeustType.NickAndEmailLogin:
                    // the ignored variables _ will used in future
                    (_result.ProfileId, _result.UserID, _, _) = StorageOperation.Persistance.NickAndEmailLogin(_request.NickName, _request.Email, _request.PasswordHash);
                    break;
                case LoginReqeustType.UniqueNickLogin:
                    (_result.ProfileId, _result.UserID, _, _) = StorageOperation.Persistance.UniqueNickLogin(_request.UniqueNick, _request.NamespaceId);
                    break;
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new LoginResponse(_request, _result);
        }
    }
}
