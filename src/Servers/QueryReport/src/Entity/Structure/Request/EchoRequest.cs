using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;

namespace UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Request
{

    public class EchoRequest : RequestBase
    {
        public EchoRequest(object rawRequest) : base(rawRequest)
        {
        }
    }
}