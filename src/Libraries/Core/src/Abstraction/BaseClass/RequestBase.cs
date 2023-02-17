using System.Xml.Serialization;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Core.Abstraction.BaseClass
{
    public abstract class RequestBase : IRequest
    {
        [XmlIgnoreAttribute]
        public object CommandName { get; protected set; }
        [XmlIgnoreAttribute]
        public object RawRequest { get; protected set; }
        public RequestBase() { }
        public RequestBase(object rawRequest)
        {
            RawRequest = rawRequest;
        }

        public abstract void Parse();
    }
}
