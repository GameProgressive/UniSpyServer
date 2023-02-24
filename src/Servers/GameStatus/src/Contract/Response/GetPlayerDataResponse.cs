using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Contract.Request;
using UniSpy.Server.GameStatus.Contract.Result;

namespace UniSpy.Server.GameStatus.Contract.Response
{
    public sealed class GetPlayerDataResponse : ResponseBase
    {
        private new GetPlayerDataResult _result => (GetPlayerDataResult)base._result;
        private new GetPlayerDataRequest _request => (GetPlayerDataRequest)base._request;
        public GetPlayerDataResponse(GetPlayerDataRequest request, GetPlayerDataResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\getpdr\1\pid\{_request.ProfileId}\lid\{_request.OperationID}\mod\1234\length\5\data\mydata\final\";
        }
    }
}
