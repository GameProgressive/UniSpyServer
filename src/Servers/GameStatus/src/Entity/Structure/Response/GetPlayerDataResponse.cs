using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Entity.Structure.Request;
using UniSpy.Server.GameStatus.Entity.Structure.Result;

namespace UniSpy.Server.GameStatus.Entity.Structure.Response
{
    public sealed class GetPlayerDataResponse : ResponseBase
    {
        private new GetPlayerDataResult _result => (GetPlayerDataResult)base._result;
        private new GetPlayerDataRequest _request => (GetPlayerDataRequest)base._request;
        public GetPlayerDataResponse(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\getpdr\1\pid\{_request.ProfileId}\lid\{_request.OperationID}\mod\1234\length\5\data\mydata\final\";
        }
    }
}
