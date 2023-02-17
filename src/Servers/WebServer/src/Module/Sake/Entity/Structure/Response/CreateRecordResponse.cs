using UniSpy.Server.WebServer.Module.Sake.Abstraction;
using UniSpy.Server.WebServer.Module.Sake.Structure.Request;
using UniSpy.Server.WebServer.Module.Sake.Structure.Result;

namespace UniSpy.Server.WebServer.Module.Sake.Structure.Response
{
    public sealed class CreateRecordResponse : ResponseBase
    {
        public new CreateRecordResult _result => (CreateRecordResult)base._result;
        public new CreateRecordRequest _request => (CreateRecordRequest)base._request;
        public CreateRecordResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            _content.Add("CreateRecordResult");
            _content.Add("tableid", _result.TableID);
            _content.Add("recordid", _result.RecordID);

            foreach (var field in _result.Fields)
            {
                _content.Add("fileds", field);
            }
            base.Build();
        }
    }
}