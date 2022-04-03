using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure;
using UniSpyServer.Servers.WebServer.Module.Sake.Structure.Request;
using UniSpyServer.Servers.WebServer.Module.Sake.Structure.Result;

namespace UniSpyServer.Servers.WebServer.Module.Sake.Structure.Response
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
            _soapBody.Add(new XElement(SoapXElement.SoapNamespace + "CreateRecord"));
            _soapBody.Add(new XElement(SoapXElement.SakeNamespace + "tableid", _result.TableID));
            _soapBody.Add(new XElement(SoapXElement.SakeNamespace + "recordid", _result.RecordID));

            foreach (var field in _result.Fields)
            {
                _soapBody.Add(new XElement(SoapXElement.SakeNamespace + "fileds", field));
            }
            base.Build();
        }
    }
}