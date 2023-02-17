using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Entity.Structure.Request;
using UniSpy.Server.GameStatus.Entity.Structure.Result;

namespace UniSpy.Server.GameStatus.Entity.Structure.Response
{
    public sealed class AuthPlayerResponse : ResponseBase
    {
        private new AuthPlayerResult _result => (AuthPlayerResult)base._result;
        private new AuthPlayerRequest _request => (AuthPlayerRequest)base._request;
        public AuthPlayerResponse(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\pauthr\{_result.ProfileId}\lid\{ _request.OperationID}\final\";
        }
    }
}
