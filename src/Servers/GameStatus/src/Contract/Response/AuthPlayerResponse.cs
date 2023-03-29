using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Contract.Request;
using UniSpy.Server.GameStatus.Contract.Result;

namespace UniSpy.Server.GameStatus.Contract.Response
{
    public sealed class AuthPlayerResponse : ResponseBase
    {
        private new AuthPlayerResult _result => (AuthPlayerResult)base._result;
        private new AuthPlayerRequest _request => (AuthPlayerRequest)base._request;
        public AuthPlayerResponse(AuthPlayerRequest request, AuthPlayerResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\pauthr\{_result.ProfileId}\lid\{ _request.LocalId}\final\";
        }
    }
}
