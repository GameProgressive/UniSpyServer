using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Contract.Request;
using UniSpy.Server.GameStatus.Contract.Result;

namespace UniSpy.Server.GameStatus.Contract.Response
{
    public sealed class AuthGameResponse : ResponseBase
    {
        private new AuthGameRequest _request => (AuthGameRequest)base._request;
        private new AuthGameResult _result => (AuthGameResult)base._result;
        public AuthGameResponse(AuthGameRequest request, AuthGameResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\sesskey\{_result.SessionKey}\lid\{ _request.LocalId}\final\";
        }
    }
}
