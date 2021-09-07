using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Structure.Request;
using GameStatus.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Response
{
    internal sealed class GetPDResponse : ResponseBase
    {
        private new GetPDResult _result => (GetPDResult)base._result;
        private new GetPDRequest _request => (GetPDRequest)base._request;
        public GetPDResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\getpdr\1\pid\{_request.ProfileID}\lid\{_request.OperationID}\mod\1234\length\5\data\mydata\final\";
        }
    }
}
