using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;

using UniSpyServer.Servers.QueryReport.Entity.Enumerate;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Request
{
    
    public class EchoRequest : RequestBase
    {
        public EchoRequest(object rawRequest) : base(rawRequest)
        {
        }
    }
}