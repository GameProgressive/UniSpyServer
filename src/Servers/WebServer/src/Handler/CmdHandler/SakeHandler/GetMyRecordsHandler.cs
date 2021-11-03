using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure.Request.SakeRequest;
using System.Linq;
using UniSpyServer.Servers.WebServer.Entity.Structure.Result;

namespace UniSpyServer.Servers.WebServer.Handler.CmdHandler
{
    public class GetMyRecordsHandler : SakeHandlerBase
    {
        private new GetMyRecordsRequest _request => (GetMyRecordsRequest)base._request;
        private new GetMyRecordsResult _result { get => (GetMyRecordsResult)base._result; set => base._result = value; }
        public GetMyRecordsHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void DataOperation()
        {
            base.DataOperation();
            _result = new GetMyRecordsResult();

            foreach (var field in _request.Fields)
            {
                var record = _sakeData.Where(x => x.FieldName == field.FieldName && x.FiledType == field.FiledType).FirstOrDefault();
                if (record != null)
                {
                    _result.Records.Add(record);
                }
            }
        }
    }
}
