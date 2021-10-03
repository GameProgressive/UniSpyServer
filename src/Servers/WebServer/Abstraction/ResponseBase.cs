using System.IO;
using System.Xml.Serialization;
using UniSpyLib.Abstraction.BaseClass;

namespace WebServer.Abstraction
{
    public abstract class ResponseBase : UniSpyResponseBase
    {
        public ResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}