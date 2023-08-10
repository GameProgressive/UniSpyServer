using UniSpy.Server.WebServer.Module.Direct2Game.Abstraction;
using UniSpy.Server.WebServer.Module.Direct2Game.Contract.Result;

namespace UniSpy.Server.WebServer.Module.Direct2Game.Contract.Request
{
    public class GetStoreAvailabilityResponse : ResponseBase
    {
        private new GetStoreAvailabilityResult _result => (GetStoreAvailabilityResult)base._result;

        public GetStoreAvailabilityResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            _content.Add("GetStoreAvailabilityResult");
            _content.Add("status");
            _content.Add("code", _result.Code);
            _content.ChangeToElement("status");
            _content.Add("storestatusid", _result.StoreStatusId);
            _content.ChangeToElement("GetStoreAvailabilityResult");
            base.Build();
        }
    }
}
