using System.Linq;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure;
using UniSpyServer.Servers.WebServer.Module.Direct2Game.Entity.Structure.Result;

namespace UniSpyServer.Servers.WebServer.Module.Direct2Game.Entity.Structure.Request
{
    public class GetStoreAvailabilityResponse : ResponseBase
    {
        private new GetStoreAvailabilityResult _result => (GetStoreAvailabilityResult)base._result;

        public GetStoreAvailabilityResponse(RequestBase request, ResultBase result) : base(request, result)
        {
            _content = new Direct2GameSoapEnvelope();
        }

        public override void Build()
        {
            _content.Add("GetStoreAvailabilityResult");
            _content.Add("status");
            _content.Add("code", _result.Status);
            _content.ChangeToElement("GetStoreAvailabilityResult");
            _content.Add("storestatusid", _result.StoreResult);
            base.Build();
        }
    }
}
