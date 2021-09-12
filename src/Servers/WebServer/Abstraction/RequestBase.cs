using System.IO;
using UniSpyLib.Abstraction.BaseClass;

namespace WebServer.Abstraction
{
    public abstract class RequestBase : UniSpyRequestBase
    {
        public new string RawRequest => (string)base.RawRequest;
        public RequestBase(string rawRequest) : base(rawRequest)
        {
        }

        protected RequestBase()
        {
        }
    }
}