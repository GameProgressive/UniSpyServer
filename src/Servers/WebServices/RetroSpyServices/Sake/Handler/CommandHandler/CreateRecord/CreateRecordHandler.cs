using WebServices.RetroSpyServices.Sake.Entity.Structure.Request;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Response;

namespace WebServices.RetroSpyServices.Sake.Handler.CommandHandler
{
    public class CreateRecordHandler : CommandHandlerBase
    {
        protected new SakeCreateRecordRequest _request;
        protected new SakeCreateRecordResponse _response;

        public CreateRecordHandler(SakeRequestBase request) : base(request)
        {
            _request = (SakeCreateRecordRequest)request;
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
        }

        public override void DataOperation()
        {
            base.DataOperation();
        }
    }
}
