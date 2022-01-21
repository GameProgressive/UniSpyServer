using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure.Request.Sake;
using UniSpyServer.Servers.WebServer.Entity.Structure.Result.Sake;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using System;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Response.Sake
{
    public sealed class CreateRecordResponse : ResponseBase
    {
        public new CreateRecordResult _result => (CreateRecordResult)base._result;
        public new CreateRecordRequest _request => (CreateRecordRequest)base._request;
        public CreateRecordResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            _soapElement.Add(new XElement(SoapXElement.SoapNamespace + "CreateRecord"));
            _soapElement.Add(new XElement(SoapXElement.SakeNamespace + "tableid", _result.TableID));
            _soapElement.Add(new XElement(SoapXElement.SakeNamespace + "recordid", _result.RecordID));

            foreach (var field in _result.Fields)
            {
                _soapElement.Add(new XElement(SoapXElement.SakeNamespace + "fileds", field));
            }
        }
    }
}