using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Entity.Structure;

namespace UniSpyServer.Servers.WebServer.Abstraction
{
    public abstract class ResponseBase : UniSpyLib.Abstraction.BaseClass.ResponseBase
    {
        protected SoapXElement _soapEnvelop { get; set; }
        protected XElement _soapBody { get; set; }
        public new string SendingBuffer { get => (string)base.SendingBuffer; set => base.SendingBuffer = value; }
        public ResponseBase(RequestBase request, ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            // Because the response is kind of soap object, so we did not use SoapXElement as a soap object
            // SoapXElement only acts like XElement
            // !! call at last
            _soapEnvelop.Add(_soapBody);
            SendingBuffer = _soapEnvelop.ToString();
        }
    }
}