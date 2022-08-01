using System.Linq;

using UniSpyServer.Servers.WebServer.Module.Sake.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Sake.Structure.Request;
using UniSpyServer.Servers.WebServer.Module.Sake.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Module.Sake.Handler
{
    
    public class GetMyRecordsHandler : CmdHandlerBase
    {
        private new GetMyRecordsRequest _request => (GetMyRecordsRequest)base._request;
        private new GetMyRecordsResult _result { get => (GetMyRecordsResult)base._result; set => base._result = value; }
        public GetMyRecordsHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            base.DataOperation();
            _result = new GetMyRecordsResult();

            foreach (var field in _request.Fields)
            {
                var record = _sakeData.FirstOrDefault(x => x.FieldName == field.FieldName && x.FiledType == field.FiledType);
                if (record is not null)
                {
                    _result.Records.Add(record);
                }
            }
        }
    }
}
