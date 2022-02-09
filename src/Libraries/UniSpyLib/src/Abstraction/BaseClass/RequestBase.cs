using System.Xml.Serialization;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass
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
            LogWriter.LogCurrentClass(this);
        }

        public abstract void Parse();
    }
}
