using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Structure.Request;
using GameStatus.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Response
{
    internal sealed class AuthResponse : GSResponseBase
    {
        private new AuthResult _result => (AuthResult)base._result;
        private new AuthRequest _request => (AuthRequest)base._request;
        public AuthResponse(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\sesskey\{_result.SessionKey}\lid\{ _request.OperationID}\final\";
        }
    }
}
